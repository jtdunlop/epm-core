/// <binding AfterBuild='copy:main' />
/// <vs />
module.exports = function (grunt) {
    grunt.initConfig({
        //this loads our packages for our grunt file
        pkg: grunt.file.readJSON('package.json'),
        copy: {
            main: {
                files: [
                    { src: 'bower_components/angular/angular.js', dest: 'wwwroot/lib/angular/angular.js' },
                    { src: 'bower_components/angular-bootstrap/ui-bootstrap-tpls.js', dest: 'wwwroot/lib/angular-bootstrap/ui-bootstrap-tpls.js' },
                    { src: 'bower_components/angular-resource/angular-resource.js', dest: 'wwwroot/lib/angular-resource/angular-resource.js' },
                    { src: 'bower_components/angular-mocks/angular-mocks.js', dest: 'wwwroot/lib/angular-mocks/angular-mocks.js' },
                    { src: 'bower_components/angular-animate/angular-animate.js', dest: 'wwwroot/lib/angular-animate/angular-animate.js' },
                    { src: 'bower_components/angular-sanitize/angular-sanitize.js', dest: 'wwwroot/lib/angular-sanitize/angular-sanitize.js' },
                    { src: 'bower_components/angular-ui-date/src/date.js', dest: 'wwwroot/lib/angular-ui-date/date.js' },
                    { src: 'bower_components/bootstrap/dist/js/bootstrap.js', dest: 'wwwroot/lib/bootstrap/bootstrap.js' },
                    { src: 'bower_components/bootstrap/dist/css/bootstrap.css', dest: 'wwwroot/lib/bootstrap/bootstrap.css' },
                    { src: 'bower_components/jquery/dist/jquery.js', dest: 'wwwroot/lib/jquery/jquery.js' },
                    { src: 'bower_components/jquery-ui/themes/base/images/*.png', dest: 'wwwroot/lib/jquery-ui/images/', expand: true, flatten: true },
                    { src: 'bower_components/jquery-ui/themes/base/jquery-ui.css', dest: 'wwwroot/lib/jquery-ui/jquery-ui.css' },
                    { src: 'bower_components/jquery-ui/jquery-ui.js', dest: 'wwwroot/lib/jquery-ui/jquery-ui.js' },
                    { src: 'bower_components/moment/moment.js', dest: 'wwwroot/lib/moment/moment.js' },
                    { src: 'bower_components/underscore/underscore.js', dest: 'wwwroot/lib/underscore.js' },
                    { src: 'bower_components/signalr/jquery.signalr.js', dest: 'wwwroot/lib/signalr/jquery.signalr.js' },
                    { src: 'bower_components/font-awesome/css/font-awesome.css', dest: 'wwwroot/lib/font-awesome/css', expand: true, flatten: true },
                    { src: 'bower_components/font-awesome/fonts/*', dest: 'wwwroot/lib/font-awesome/fonts', expand: true, flatten: true },
                    { src: 'bower_components/jasmine/lib/jasmine-core/*', dest: 'wwwroot/lib/jasmine', expand: true, flatten: true },
                    { src: 'bower_components/jasmine/images/*', dest: 'wwwroot/images/jasmine', expand: true, flatten: true }
                ]
            }
        }
    });

    //npm modules need for our task
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.registerTask('default', ['copy']);
};