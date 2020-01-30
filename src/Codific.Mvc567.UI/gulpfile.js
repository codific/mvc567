// This file is part of the mvc567 distribution (https://github.com/codific/mvc567).
// Copyright (C) 2019 Codific Ltd.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

/// <binding />
'use strict'

var gulp = require("gulp"),
    sass = require('gulp-sass'),
    fs = require("fs"),
    concat = require('gulp-concat'),
    cssmin = require('gulp-cssmin'),
    htmlmin = require('gulp-htmlmin'),
    uglify = require('gulp-uglify-es').default,
    merge = require('merge-stream'),
    del = require('del'),
    replace = require('gulp-replace'),
    bundleconfig = require('./bundleconfig.json');

const regex = {
    css: /\.css$/,
    html: /\.(html|htm)$/,
    js: /\.js$/,
    cshtml: /\.(cshtml|cshtm)$/,
};

gulp.task('styles', () =>
    gulp
        .src([
            './Styles/scss/shared/style.scss',
            './Styles/scss/admin/style.scss'
        ])
        .pipe(sass())
        .pipe(cssmin({ keepSpecialComments: 0 }))
        .pipe(concat('style.min.css'))
        .pipe(gulp.dest('./Styles/css'))
);

gulp.task('styles:vendors', () =>
    gulp
        .src([
            './node_modules/bootstrap/dist/css/bootstrap.min.css',
            './node_modules/perfect-scrollbar/css/perfect-scrollbar.css',
            './Styles/css/shared/editor/codemirror.css',
            './Styles/css/shared/editor/foldgutter.css',
            './Styles/css/shared/editor/dialog.css',
            './Styles/css/shared/editor/eclipse.css',
            './node_modules/jquery-tags-input/dist/jquery.tagsinput.min.css',
            './Styles/vendors/bootstrap-editable.css',
            './Scripts/js/vendors/bootstrap-datepicker/bootstrap-datepicker.min.css',
        ])
        .pipe(cssmin({ keepSpecialComments: 0 }))
        .pipe(concat('style.vendors.min.css'))
        .pipe(gulp.dest('./Styles/css'))
);

gulp.task('scripts', () =>
    gulp.src([
        "./Scripts/js/shared/off-canvas.js",
        "./Scripts/js/shared/hoverable-collapse.js",
        "./Scripts/js/shared/misc.js",
        "./Scripts/js/shared/todolist.js",
        "./Scripts/js/shared/tooltips.js",
        "./Scripts/js/shared/popover.js",
        "./Scripts/js/shared/clipboard.js",
        "./Scripts/js/shared/form-addons.js",
        "./Scripts/js/shared/formpickers.js",
        "./Scripts/js/shared/upload-file-input.js",
        "./Scripts/js/shared/editor/codemirror.js",
        "./Scripts/js/shared/editor/*.js",
        "./Scripts/js/shared/obfuscator.js",
        "./Scripts/js/shared/static-page-form.js",
        "./Scripts/js/shared/x-editable.js",
    ])
        .pipe(uglify())
        .pipe(concat("scripts.min.js"))
        .pipe(gulp.dest('./Scripts/js/'))
);

gulp.task('scripts:vendors', () =>
    gulp.src([
        './Scripts/js/vendors/vendor.bundle.base.js',
        './node_modules/jquery/dist/jquery.min.js',
        './node_modules/popper.js/dist/umd/popper.min.js',
        './node_modules/bootstrap/dist/js/bootstrap.min.js',
        './node_modules/perfect-scrollbar/dist/perfect-scrollbar.min.js',
        './node_modules/clipboard/dist/clipboard.min.js',
        './node_modules/jquery-tags-input/dist/jquery.tagsinput.min.js',
        './node_modules/sweetalert/dist/sweetalert.min.js',
        './Scripts/js/vendors/bootstrap-editable.min.js',
        './Scripts/js/vendors/bootstrap-datepicker/bootstrap-datepicker.min.js',
    ])
        .pipe(uglify())
        .pipe(concat("scripts.vendors.min.js"))
        .pipe(gulp.dest('Scripts/js/'))
);

gulp.task('concat:cshtml', () => {
    return merge(getBundles(regex.cshtml).map(bundle => {
        return gulp.src(bundle.inputFiles, { base: '.' })
            .pipe(concat(bundle.outputFileName))
            .pipe(replace('@', '@@'))
            .pipe(htmlmin({ collapseWhitespace: true }))
            .pipe(gulp.dest('.'));
    }))
});

gulp.task('clean', () => 
    del(bundleconfig.map(bundle => bundle.outputFileName))
);

gulp.task('default:style', gulp.series('styles', 'styles:vendors', 'concat:cshtml'));
gulp.task('default', gulp.series('clean', 'styles', 'styles:vendors', 'scripts', 'scripts:vendors', 'concat:cshtml'));

const getBundles = (regexPattern) => {
    return bundleconfig.filter(bundle => {
        return regexPattern.test(bundle.outputFileName);
    });
};