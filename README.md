# aspnet-buzz

## Developing on `*nix`

__Install__ [TSD](http://definitelytyped.org/tsd/) and [gulp](http://gulpjs.com/) globally using `npm`

```
npm install tsd -g
npm install gulp -g
```

Restore required dependencies

```
tsd reinstall
npm install
dnu restore
```

To build and run the app

```
gulp bundle
dnx . web
```

Then open `http://localhost:5004` in a browser and marvel at the wonderful world of ASP.NET running on `*nix` :tada:

## Developing on `Windows`

:construction_worker: