#!/bin/bash
rm -rf artifacts
if ! type dnvm > /dev/null 2>&1; then
    curl -sSL https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.sh | DNX_BRANCH=dev sh && source ~/.dnx/dnvm/dnvm.sh
fi
dnvm upgrade
dnu restore
rc=$?; if [[ $rc != 0 ]]; then exit $rc; fi
dnu publish src/aspnet-buzz --no-source --out artifacts/build/aspnet-buzz 2>&1 | tee buildlog
# work around for kpm bundle returning an exit code 0 on failure 
grep "Build succeeded" buildlog
