﻿(function () {
    'use strict';
    var controllerId = 'reports.apps.expired.controller';

    angular
        .module('app.reports')
        .controller('ReportsAppsExpiredController', ReportsAppsExpiredController);

    ReportsAppsExpiredController.$inject = ['$q', 'ApplicationDataService', 'usSpinnerService', 'logger', '$log', '$timeout'];

    function ReportsAppsExpiredController($q, ApplicationDataService, usSpinnerService, logger, $log, $timeout) {
        var vm = this;
        vm.applications = [];
        vm.pageSize = 10;
        vm.query = "";
        vm.currentPage = 1;
        vm.loading = false;
        vm.isMenuOpen = false;
        vm.csvExportFileName = "ApplicationsExpired";
        vm.reportFields = {appId: 'Application ID', displayName: 'Display Name', replyUrls: 'Reply Url', identifierUris: 'IdentifierUris', endDate: 'End Date'};
    

        vm.getExpiredApplications = getExpiredApplications;
        vm.getExpiredApplicationCount = getExpiredApplicationCount;
        vm.setPageSize = setPageSize;
        vm.openMenu = openMenu;

        /*Have to do this for spinner because $broadcast loads first */
        $timeout(function () {
            usSpinnerService.spin('spinner');
        }, 100);

        activate();

        function activate() {
            logger.info('Activating Expired Applications');
            vm.loading = true;
            usSpinnerService.spin('spinner');
            getExpiredApplications();
        }

        function getExpiredApplications() {

            $log.info('Info ' + controllerId, 'Entering getAllExpiredPrincipals');
            return ApplicationDataService.getExpiredApplications()
           .then(function (data) {
               vm.applications = data;
           		vm.loading = false;
           		usSpinnerService.stop('spinner');
           		return vm.applications;
           });
        }

        function getExpiredApplicationCount() {
            return vm.applications.length;
        }
        function openMenu() {
            vm.isMenuOpen = !vm.isMenuOpen;
        }

        function setPageSize(pageSize) {
            vm.pageSize = pageSize;
        }
    }
})();