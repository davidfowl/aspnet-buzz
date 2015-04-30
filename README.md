# aspnet-buzz

## Developing on `*nix`

__Install__ [TSD](http://definitelytyped.org/tsd/) and [gulp](http://gulpjs.com/) globally using `npm`

```
[sudo] npm install tsd -g
[sudo] npm install gulp -g
```

Restore required dependencies

```
[sudo] tsd reinstall
[sudo] npm install
[sudo] dnu restore
```

To build and run the app

```
gulp bundle
dnx . web
```

Then open `http://localhost:5004` in a browser and marvel at the wonderful world of ASP.NET running on `*nix` :tada:

#### Gist with GitHub events that can be used for testing

https://gist.github.com/alfhenrik/b422af0213be4b628836

## Developing on `Windows`

:construction_worker: