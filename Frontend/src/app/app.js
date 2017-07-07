import angular from 'angular';

import '../style/app.css';
import 'angular-route';
import 'angular-messages';
import 'angular-ui-bootstrap';

let app = () => {
  return {
    template: require('./app.html'),
    controller: 'AppCtrl',
    controllerAs: 'app'
  }
};

class AppCtrl {
  constructor() {
  }
}

const MODULE_NAME = 'angularApp';

var peopleTemplateUrl = require('../views/people.html');
var personTemplateUrl = require('../views/add.html');
var personEditTemplateUrl = require('../views/edit.html');

var angularApp = angular.module(MODULE_NAME, ['ngRoute', 'ngMessages', 'ui.bootstrap'])
    .directive('app', app)
    .controller('AppCtrl', AppCtrl)
    .config(function($routeProvider){
      $routeProvider.when('/add',
          {
            template: personTemplateUrl,
            controller:'AddController'
          });
      $routeProvider.when('/edit/:id',
          {
            template: personEditTemplateUrl,
            controller:'EditController'
          });
      $routeProvider.when('/people',
          {
            template: peopleTemplateUrl,
            controller:'PeopleController'
          });
      $routeProvider.otherwise({redirectTo: '/people'});
    });

require('../services/');
require('../controllers');

export default MODULE_NAME;