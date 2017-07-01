'use strict';
function dataService($http, $q){
    return{
        getPeople: function(filter){
            var deferred = $q.defer();
            $http({method: 'GET', url: 'http://localhost:34703/api/people',
                params: {query: filter.query, offset: filter.offset, limit: filter.limit, orderByDesc: filter.orderByDesc }}).
            then (function success(response) {
                    deferred.resolve(response.data);
                }, function error(response) {
                    deferred.reject(response.status);
                }
            );

            return deferred.promise;
        },
        getGroups: function (filter) {
            var deferred = $q.defer();
            $http({method: 'GET', url: 'http://localhost:34703/api/groups',
                params: {offset: filter.offset, limit: filter.limit }}).
            then (function success(response) {
                    deferred.resolve(response.data);
                }, function error(response) {
                    deferred.reject(response.status);
                }
            );
            return deferred.promise;
        }
    }
}
module.exports = dataService;