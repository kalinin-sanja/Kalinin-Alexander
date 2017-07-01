'use strict';
function PeopleController($scope, dataService) {
    $scope.pageSize = 5;
    $scope.currentPage = 1;

    var filter = {};
    filter.limit = $scope.pageSize;

    $scope.setSearchQuery = function (query) {
        $scope.currentPage = 1;
        updatePage(query);
    };

    function updatePage(query) {
        filter.query = query;
        filter.offset = ($scope.currentPage - 1) * filter.limit;
        getData(filter);
    }

    function getData(filter) {
        var promiseObj = dataService.getPeople(filter);
        promiseObj.then(function (value) {
            $scope.people = value.people;
            $scope.totalCount = value.totalCount;
        });
    }

    $scope.pageChanged = function () {
        updatePage($scope.query);
    };

    $scope.orderReverse = false;

    $scope.sort = function () {
        $scope.orderReverse = !$scope.orderReverse;
        filter.orderByDesc = $scope.orderReverse;
        $scope.currentPage = 1;
        $scope.pageChanged();
    };

    $scope.isSortUp = function () {
        return !$scope.orderReverse;
    };

    $scope.isSortDown = function () {
        return $scope.orderReverse;
    };

    updatePage(null);
}
module.exports = PeopleController;