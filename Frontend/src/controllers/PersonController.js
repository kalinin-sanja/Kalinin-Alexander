'use strict';
function PersonController($scope, $http, $location, dataService){
        $scope.error = false;
        $scope.person = {};
        $scope.person.sex = true;
        function getGroups(filter) {
            var promiseObj=dataService.getGroups(filter);
            promiseObj.then(function(value) {
                $scope.groups = value;
                $scope.group = $scope.groups[0];
                $scope.person.groupId = $scope.group.id;
            });
        }
        $scope.updateGroupId = function (id) {
            $scope.person.groupId = id;
        };

        var filter = {};
        getGroups(filter);
        $scope.response={};
        $scope.save = function (person, personForm){
            if(personForm.$valid){
                $http.post("http://localhost:34703/api/people/", person).then(function success(response) {
                    $scope.response=response.data;
                    $location.path('/people');
                },
                function errorCallback(response) {
                    $scope.error = true;
                });
            }
            else
            {
                $scope.personForm.birthday.$touched = true;
            }
        };

    $scope.today = function() {
        $scope.person.birthday = new Date();
    };
    $scope.today();

    $scope.clear = function() {
        $scope.person.birthday = null;
    };

    $scope.inlineOptions = {
        customClass: getDayClass,
        minDate: new Date(),
        showWeeks: true
    };

    $scope.dateOptions = {
        formatYear: 'yyyy',
        maxDate: new Date(2020, 5, 22),
        minDate: new Date(),
        startingDay: 1
    };

    $scope.toggleMin = function() {
        $scope.inlineOptions.minDate = $scope.inlineOptions.minDate ? null : new Date();
        $scope.dateOptions.minDate = $scope.inlineOptions.minDate;
    };

    $scope.toggleMin();

    $scope.openCalendar = function() {
        $scope.popupCalendar.opened = true;
    };

    $scope.setDate = function(year, month, day) {
        $scope.person.birthday = new Date(year, month, day);
    };

    $scope.format = 'yyyy/MM/dd';

    $scope.popupCalendar = {
        opened: false
    };

    var tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);
    var afterTomorrow = new Date();
    afterTomorrow.setDate(tomorrow.getDate() + 1);
    $scope.events = [
        {
            date: tomorrow,
            status: 'full'
        },
        {
            date: afterTomorrow,
            status: 'partially'
        }
    ];

    function getDayClass(data) {
        var date = data.date,
            mode = data.mode;
        if (mode === 'day') {
            var dayToCheck = new Date(date).setHours(0,0,0,0);

            for (var i = 0; i < $scope.events.length; i++) {
                var currentDay = new Date($scope.events[i].date).setHours(0,0,0,0);

                if (dayToCheck === currentDay) {
                    return $scope.events[i].status;
                }
            }
        }

        return '';
    }
}
module.exports = PersonController;