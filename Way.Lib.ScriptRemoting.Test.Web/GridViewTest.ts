declare var grid: WayGridView;
var testPage: WayScriptRemoting;

window.onload = () => {
    testPage = (<any>window).controller;
    testPage.onmessage = (e) => {
        alert("group msg:" + e);
    }
    testPage.groupName = "g1";


    //grid.header = new WayTemplate( '<tr><td>H1</td><td>H2</td></tr>');
    //grid.footer = new WayTemplate('<tr><td>footer1</td><td>F2</td></tr>');
    //{$ItemIndex}代表当前Item的索引号
    //grid.searchModel = WayDataBindHelper.dataBind(<any>"searchDiv", {});
    //grid.onItemSizeChanged = function () {
    //    //注意，headertable里面的td不要设置width，width要设置在item里的td
    //    var tableSource = $("#grid1");
    //    var tableHeader = tableSource.prev();
    //    var tableFooter = tableSource.next();
    //    //保持两个table的td同宽
    //    grid.setSameWidthForTables(tableSource, tableHeader);
    //    grid.setSameWidthForTables(tableSource, tableFooter);
    //}

    grid.itemStatusModel = {
        Selected: false,
        onchange: function (model, itemIndex, name, value) {

            if (name == "Selected") {
                model.Color = value ? "#cccccc" : "white";
            }

        }
    };
    grid.onAfterCreateItems = function (total, hasmore) {
        $("#tdfooter").html("记录数：" + (hasmore ? "大于" : "共") + total);
    }
    grid.onDatabind = function () {
        grid.sum(["TableID"], function (data, err) {
            if (err)
                alert(err);
            else {
                document.getElementById("hjDiv").innerHTML = "合计TableID：" + data.TableID;
            }
        });

        grid.count(function (data, err) {
            if (err)
                alert(err);
            else {
                document.getElementById("totalDiv").innerHTML = "总记录数：" + data;
            }
        });
    }
    grid.databind();
}

function additem() {
    grid.addItem({
        Name:"newName",
        caption: "test one",
        dbType: "new type",
        TableID: {
            value: 998,
            text:"tableName",
        },
    });
}

function goSearch() {
    grid.databind();
}

function edit(itemIndex) {
    try {
        grid.changeMode(itemIndex, "edit");

    }
    catch (e) {
        alert(e.message);
    }
}
function save(editingIndex) {
    try {
        grid.save(editingIndex, function (ret, err) {
            if (err)
                alert(err);
            else {
                grid.rebindItemFromServer(editingIndex, "");
                // grid.acceptItemChanged(editingIndex);
                //grid.changeMode(editingIndex, "");
            }
        });

    }
    catch (e) {
        alert(e.message);
    }

}
function normal(itemIndex) {
    try {
        grid.changeMode(itemIndex, "");
    }
    catch (e) {
        alert(e.message);
    }
}


function uploadfile() {
    var file1 = document.getElementById("file1");
    var handler = testPage.uploadFile(file1, "my state", (ret, totalSize, uploaded, err) => {
        if (err) {
            alert("uploadfile err:" + err);
        }
        else {
            if (uploaded == totalSize) {
                document.title = "upload completed";
            }
            else {
                document.title = ((uploaded * 100) / totalSize) + "%";
            }
        }
    }, null);
}