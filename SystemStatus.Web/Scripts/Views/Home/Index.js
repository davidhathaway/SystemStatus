/// <reference path="../../typings/knockout/knockout.d.ts" />
/// <reference path="../../typings/jquery/jquery.d.ts" />
/// <reference path="../../typings/jquery.timeago/jquery.timeago.d.ts" />
/// <reference path="../../typings/signalr/signalr.d.ts" />
/// <reference path="../../typings/bootstrap/bootstrap.d.ts" />
/// <reference path="../../typings/flot/jquery.flot.d.ts" />
/// <reference path="../../typings/moment/moment.d.ts" />
/// <reference path="../common.ts" />
$(function () {
    var appModel = new SystemAppViewKoModel();
    appModel.Load(rawSystemStatusModel);
    ko.applyBindings(appModel);
    var hub = $.connection.appEventHub;
    hub.client.updateAppStatus = function (model) {
        console.log(model);
        appModel.UpdateItem(model);
    };
    $.connection.hub.start().done(function (e) {
        console.log(e);
    });
});
//# sourceMappingURL=Index.js.map