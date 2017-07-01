'use strict';
function NavController($scope, $location) {
    $scope.isActive = function(viewLocation) {
        return viewLocation === $location.path();
    };

    $scope.classActive = function( viewLocation ) {
        if( $scope.isActive(viewLocation) ) {
            return 'active';
        }
        else {
            return '';
        }
    }
}
module.exports = NavController;