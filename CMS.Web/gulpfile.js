const { series, parallel } = require('gulp');
const gulp = require('gulp');
const sass = require('gulp-sass')
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
        src: "wwwroot/assets/src/js/**/*.ts",
        dest: "wwwroot/assets/dist/js"
    },
    img: {
        src: "wwwroot/assets/src/images/**/*",
        dest: "wwwroot/assets/dist/images"
    },
    lib: {
        src: "wwwroot/assets/vendor"
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
        .src(paths.js.src)
        .pipe(tsProject())
        .pipe(concat('web-bundle.js'))
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
        paths.lib.src + '/quilljs/quill.snow.css'
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
        paths.lib.src + '/bootstrap/js/bootstrap.min.js'
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
        paths.lib.src + '/quilljs/quill.min.js'
    ])
        .pipe(tsProject())
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
exports.js = series(javascript, libJavascript, libAdminJavascript);
exports.build = series(clean, series(series(css, javascript), series(libCss, libJavascript, libAdminCss, libAdminJavascript)), images);