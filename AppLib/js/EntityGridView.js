
function _EntityGridView_HeaderCheck(sender, name) {
            var value = sender.checked;
            var chks = document.getElementsByName(name);
            for (var i = 0 ; i < chks.length ; i++) {
                chks[i].checked = value;
            }
}

function _EntityGridView_submit() {
    var form = document.forms[0];

    if (form.fireEvent) {
        form.fireEvent("onsubmit");
        form.submit();
    }
    else {
        var evt = document.createEvent('HTMLEvents');
        evt.initEvent('submit', true, true);
        form.dispatchEvent(evt);
        form.submit();
    }
}

		function _EntityGridView_Search(clientID) {
			var obj = document.getElementsByName(clientID + "_$$_command")[0];
            obj.value = "search";
            _EntityGridView_submit();
		}
		function _EntityGridView_DataBind(clientID) {
		    var obj = document.getElementsByName(clientID + "_$$_command")[0];
		    obj.value = "rebind";
		    _EntityGridView_submit();
		}

		function _EntityGridView_HandleChecked(clientID, idfield) {

		    var obj = document.getElementsByName(clientID + "_$$_command")[0];
		    obj.value = "HandleChecked";

		    _EntityGridView_submit();
		}

		function _EntityGridView_Delete(clientID,idfield,noAsk) {
		var has2del = false;
			 var chks = document.getElementsByName(clientID + "_chk_" + idfield);
            for (var i = 0 ; i < chks.length ; i++) {
				if(chks[i].checked)
               {
			   has2del = true;
			   break;
			   }
            }
			if(!has2del)
			{
			    if (!noAsk)
				alert("请打勾需要删除的数据");
				return;
			}
			if (noAsk) {
			    var obj = document.getElementsByName(clientID + "_$$_command")[0];
			    obj.value = "delete";

			    _EntityGridView_submit();
			}
			else if( has2del && confirm("确定删除打勾的数据吗？") ){
                var obj = document.getElementsByName(clientID + "_$$_command")[0];
                obj.value = "delete";
			
                _EntityGridView_submit();
			}
        }
		function _EntityGridView_RowClick(clientID,index) {
			var obj = document.getElementsByName(clientID + "_$$_command")[0];
            obj.value = "click:" + index;
            _EntityGridView_submit();
		}
		function _EntityGridView_Order(clientID, field) {
		    var obj = document.getElementsByName(clientID + "_$$_command")[0];
		    obj.value = "order:" + field;
		    _EntityGridView_submit();
		}

        
		
		function _EntityGridView_SetStyle(clientID, rowClickedColor, rowOverColor) {
		    var obj = document.getElementById(clientID);
		    obj.setAttribute("_EntityGridView", "1");
		    obj.style.marginBottom = "26px";
		    var lastClickedRow = false;

		    function rowMouseOut(event) {
		        var row = event.target;
		        while (row.tagName != "TR" && row.parentElement) {
		            row = row.parentElement;
		        }

		        if (row != lastClickedRow) {
		            row.style.backgroundColor = row.getAttribute("_OldBgColor");
		        }
		    }
		    function rowMouseOver(event) {
		        var row = event.target;
		        while (row.tagName != "TR" && row.parentElement) {
		            row = row.parentElement;
		        }

		        if (row != lastClickedRow) {
		            row.style.backgroundColor = rowOverColor;
		        }
		    }
		    function rowMouseClick(event) {
		        var row = event.target;
		        while (row.tagName != "TR" && row.parentElement) {
		            row = row.parentElement;
		        }
		        if (lastClickedRow && lastClickedRow != row) {
		            lastClickedRow.style.backgroundColor = lastClickedRow.getAttribute("_OldBgColor");
		        }
		       
		        row.style.backgroundColor = rowClickedColor;
		        lastClickedRow = row;
		    }

		    var rows = obj.rows;
		    for (var i = 0 ; i < rows.length ; i++) {
		        var row = rows[i];
		        if (row.getAttribute("_RowType") == "DataRow") {
		            row.setAttribute("_OldBgColor", row.style.backgroundColor);
		            $(row).mouseover(rowMouseOver);
		            $(row).mouseout(rowMouseOut);
		            $(row).click(rowMouseClick);
		        }
		    }
		}