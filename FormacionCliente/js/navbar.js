
(function () {
    'use strict';

    angular
        .module('routerApp')
        .config(PanelProviderConfig)
        .controller('PanelProviderCtrl', PanelProviderCtrl)
        .controller('PanelMenuCtrl', PanelMenuCtrl);

    function PanelProviderConfig($mdPanelProvider) {
        $mdPanelProvider.definePreset('demoPreset', {
            attachTo: angular.element(document.body),
            controller: PanelMenuCtrl,
            controllerAs: 'ctrl',
            templateUrl: 'templates/menu.html',
            panelClass: 'menu-panel-container',
            focusOnOpen: false,
            zIndex: 100,
            clickOutsideToClose: true,
            propagateContainerEvents: true,
            groupName: 'menus',
            hasBackdrop: true,
            escapeToClose: true,
            focusOnOpen: true
        });
    }

    function PanelProviderCtrl($mdPanel) {

        this.navigation = {
            name: 'navigation'
        };

        this.showMenu = function ($event, menu) {

            $mdPanel.open('demoPreset', {
                id: 'menu_' + menu.name,
                position: $mdPanel.newPanelPosition()
                    .relativeTo($event.target)
                    .addPanelPosition(
                      $mdPanel.xPosition.ALIGN_START,
                      $mdPanel.yPosition.BELOW
                    ),
                locals: {
                    items: menu.items
                },
                openFrom: $event
            });
        };
    }

    function PanelMenuCtrl(mdPanelRef) {
        this.closeMenu = function () {
            mdPanelRef && mdPanelRef.close();
        };
    }

})
();