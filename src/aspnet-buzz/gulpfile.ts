/// <reference path="node_modules/typescript-formatter/typescript-formatter.d.ts"/>
/// <reference path="typings/gulp/gulp.d.ts"/>
/// <reference path="typings/gulp-tslint/gulp-tslint.d.ts"/>
/// <reference path="typings/gulp-typescript/gulp-typescript.d.ts"/>

import gulp = require("gulp");
import path = require("path");
import glob = require("glob");
import del = require("del");
import dts = require("dts-bundle");
import tslint = require("gulp-tslint");
import typescript = require("gulp-tsc");
import tsfmt = require("typescript-formatter");
import browserify = require("browserify");
import source = require("vinyl-source-stream");

/**
 * Directories
 */
var dirs = {
    build: "build",
    dist: "wwwroot",
    src: "app",
    typings: "typings"
}

/**
 * Globs
 */
var globs = {
    all: path.join("**", "*"),
    gulp: "gulpfile.ts",
    bundle: "bundle.js",
    js: path.join("**", "*.js"),
    ts: path.join("**", "*.ts")
}

var name = "app";
var scripts = path.join(dirs.src, globs.ts);

/**
 * Combine the dirs for minimatch
 * @param dirs
 * @returns {string}
 */
function combine(...dirs: string[]): string {
    return "{" + dirs.join(",") + "}";
}

gulp.task("clean", (callback) => {
    del([dirs.build], callback);
});

gulp.task("bundle", ["scripts"], () => {
    return browserify("./" + dirs.build + "/app.js")
        .bundle()
        //Pass desired output filename to vinyl-source-stream
        .pipe(source(globs.bundle))
        // Start piping stream to tasks!
        .pipe(gulp.dest(dirs.dist));
});

gulp.task("copy", ["scripts"], () => {
    return gulp.src(path.join(dirs.build, globs.all))
        .pipe(gulp.dest(dirs.dist));
});

gulp.task("format", (callback) => {
    glob(scripts, (err, files) => {
        if(err) {
            return callback(err);
        }
        files.push(globs.gulp);
        tsfmt.processFiles(files, {
            editorconfig: false,
            replace: true,
            tsfmt: true,
            tslint: true
        });
        return callback(null);
    });
});

gulp.task("lint", () => {
    return gulp.src([globs.gulp, globs.ts])
        .pipe(tslint())
        .pipe(tslint.report("verbose", {
            emitError: true
        }));
});

gulp.task("scripts", ["clean"], (callback) => {
    return gulp.src([scripts, path.join(dirs.typings, globs.ts)])
        .pipe(typescript())
        .pipe(gulp.dest(dirs.build));
});