const { series, parallel } = require('gulp');
const gulp = require('gulp');
const sass = require('gulp-sass')(require('sass'));
const postcss = require("gulp-postcss");
const autoprefixer = require("autoprefixer");
const cssnano = require("cssnano");
const sourcemaps = require("gulp-sourcemaps");
const ts = require('gulp-typescript');
const typescript = require('typescript');
const imagemin = require("gulp-imagemin");
const rename = require("gulp-rename");
const del = require("del");
const concat = require('gulp-concat');
let terser = require('gulp-terser');

let tsProject = ts.createProject('tsconfig.json', { noImplicitAny: true });

let paths = {
    css: {
        src: "wwwroot/assets/src/scss/**/*.scss",
        dest: "wwwroot/assets/dist/css"
    },
    js: {
        dest: "wwwroot/assets/dist/js"
    },
    web_js: {
        src: "wwwroot/assets/src/js/web/**/*.ts"
    },
    admin_js: {
        src: "wwwroot/assets/src/js/admin/**/*.ts",
    },
    img: {
        src: "wwwroot/assets/src/images/**/*",
        dest: "wwwroot/assets/dist/images"
    },
    lib: {
        src: "wwwroot/assets/vendor"
    },
    node: {
        src: "node_modules"
    }
};

function clean() {
    return del(['wwwroot/assets/dist']);
}

function css() {
    return gulp
        .src(paths.css.src)
        .pipe(sourcemaps.init())
        .pipe(sass({ outputStyle: "expanded" }))
        .on("error", sass.logError)
        .pipe(rename({ suffix: ".min" }))
        .pipe(postcss([autoprefixer(), cssnano()]))
        .pipe(sourcemaps.write())
        .pipe(gulp.dest(paths.css.dest));
}

function javascript() {
    return gulp
        .src(paths.web_js.src)
        .pipe(tsProject())
        .pipe(concat('web-bundle.js'))
        .pipe(rename({ suffix: ".min" }))
        .pipe(sourcemaps.init({ loadMaps: true }))
        .pipe(terser())
        .pipe(sourcemaps.write())
        .pipe(gulp.dest(paths.js.dest));
}

function adminJavascript() {
    return gulp
        .src(paths.admin_js.src)
        .pipe(tsProject())
        .pipe(concat('admin-bundle.js'))
        .pipe(rename({ suffix: ".min" }))
        .pipe(sourcemaps.init({ loadMaps: true }))
        .pipe(terser())
        .pipe(sourcemaps.write())
        .pipe(gulp.dest(paths.js.dest));
}

function libCss() {
    return gulp.src([
        paths.lib.src + '/bootstrap/scss/bootstrap.scss'
    ])
        .pipe(sourcemaps.init())
        .pipe(sass({ outputStyle: "expanded" }))
        .on("error", sass.logError)
        .pipe(concat('lib-bundle.css'))
        .pipe(rename({ suffix: ".min" }))
        .pipe(postcss([autoprefixer(), cssnano()]))
        .pipe(sourcemaps.write())
        .pipe(gulp.dest(paths.css.dest));
}

function libAdminCss() {
    return gulp.src([
        paths.lib.src + '/bootstrap/scss/bootstrap.scss',
        paths.lib.src + '/bootstrap/scss/bootstrap-grid.scss',
        paths.lib.src + '/bootstrap/scss/bootstrap-reboot.scss',
        paths.lib.src + '/bootstrap/scss/bootstrap-utilities.scss',
        paths.lib.src + '/quilljs/quill.snow.css',
        paths.lib.src + '/quilljs/quill-resize-module/resize.css'
    ])
        .pipe(sourcemaps.init())
        .pipe(sass({ outputStyle: "expanded" }))
        .on("error", sass.logError)
        .pipe(concat('lib-admin-bundle.css'))
        .pipe(rename({ suffix: ".min" }))
        .pipe(postcss([autoprefixer(), cssnano()]))
        .pipe(sourcemaps.write())
        .pipe(gulp.dest(paths.css.dest));
}

function libJavascript() {
    return gulp.src([
        paths.lib.src + '/bootstrap/js/bootstrap.bundle.min.js',
    ])
        .pipe(tsProject())
        .pipe(concat('lib-bundle.js'))
        .pipe(rename({ suffix: ".min" }))
        .pipe(sourcemaps.init({ loadMaps: true }))
        .pipe(terser())
        .pipe(sourcemaps.write())
        .pipe(gulp.dest(paths.js.dest));
}

function libAdminJavascript() {
    return gulp.src([
        paths.lib.src + '/bootstrap/js/bootstrap.bundle.min.js',
        paths.lib.src + '/quilljs/quill.min.js',
        paths.lib.src + '/quilljs/quill-resize-module/resize.js',
        paths.lib.src + '/quilljs/htmlEdit/highlight.min.js',
        paths.lib.src + '/quilljs/htmlEdit/quill.htmlEditButton.min.js',
        paths.lib.src + '/quilljs/quill.imageUploader.min.js',
        // paths.lib.src + '/quilljs/quill.imageCompressor.min.js'
    ])
        .pipe(tsProject())
        //.pipe(ts({noImplicitAny: true, out: "output.js"}))
        .pipe(concat('lib-admin-bundle.js'))
        .pipe(rename({ suffix: ".min" }))
        .pipe(sourcemaps.init({ loadMaps: true }))
        .pipe(terser())
        .pipe(sourcemaps.write())
        .pipe(gulp.dest(paths.js.dest));
}

function images() {
    return gulp
        .src(paths.img.src)
        // .pipe(newer("./_site/assets/img"))
        .pipe(
            imagemin([
                imagemin.gifsicle({ interlaced: true }),
                // imagemin.mozjpeg({ progressive: true, }),
                imagemin.mozjpeg({ quality: 95 }),
                imagemin.optipng({ optimizationLevel: 5 }),
                imagemin.svgo({
                    plugins: [
                        {
                            removeViewBox: false,
                            collapseGroups: true
                        }
                    ]
                })
            ])
        )
        .pipe(gulp.dest(paths.img.dest));
}

function watch() {
    gulp.watch(paths.img.src, images);
    gulp.watch(paths.css.src, css);
    gulp.watch(paths.js.src, javascript);
}

exports.images = images;
exports.watch = watch;
exports.css = series(css, libCss, libAdminCss);
exports.js = series(javascript, libJavascript, adminJavascript, libAdminJavascript);
exports.build = series(clean, series(series(css, javascript, adminJavascript), series(libCss, libJavascript, libAdminCss, libAdminJavascript)), images);