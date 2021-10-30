app.filter('byteToKB', function () {
    return function (input) {
        if (typeof input == "undefined") {
            return 0 + ' KB';
        }
        else {
            if (Number(input) == 0) {
                return 0 + ' KB';
            } else {
                let bytes = Number(input);
                let decimals = 0;
                const k = 1024;
                const dm = decimals < 0 ? 0 : decimals;
                const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
                const i = Math.floor(Math.log(bytes) / Math.log(k));
                const fs = Math.ceil(parseFloat((bytes / Math.pow(k, i))));
                const kbs = fs.toFixed(dm) + ' ' + sizes[i];
                return kbs;
            }
        }
    }
});