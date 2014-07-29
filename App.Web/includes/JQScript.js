// JScript 文件
 //全选页面有相似ID的checkbox
        function SelectAllCheckBoxInThisPageByID(tempControl)
       {
           //将除头模板中的其它所有的CheckBox取反 
            var theBox=tempControl;
             xState=theBox.checked;    

            elem=theBox.form.elements;
            for(i=0;i<elem.length;i++)
            if(elem[i].type=="checkbox" && elem[i].id!=theBox.id && elem[i].id.indexOf("chkSelect")>-1)
             {
                    elem[i].checked=xState;
            }
        }
//取消页面其他radiobutton的选择
	function UnselectOtherRadioInThisPage(tempControl)
       {
            elem=tempControl.form.elements;            
            for(i=0;i<elem.length;i++)
            if(elem[i].type=="radio" && elem[i].id!=tempControl.id && elem[i].id.indexOf("rbtSelect") > -1)
            {
                    elem[i].checked=false;
            }
        }
         function UnselectOtherRadioInThisPageS(tempControl)
       {
            elem=tempControl.form.elements;            
            for(i=0;i<elem.length;i++)
            if(elem[i].type=="radio" && elem[i].id!=tempControl.id && elem[i].id.indexOf("rbton") > -1)
            {
                    elem[i].checked=false;
            }
        }
        function UnselectOtherRadioInSetedControl(tempControl,ContainerControlClientID)
       {
            var elem=document.getElementsByTagName("input");
       
            //var elem=document.getElementById(ContainerControlClientID).childNodes;
            for(i=0;i<elem.length;i++)
            if(elem[i].type=="radio" && elem[i].id!=tempControl.id && elem[i].id.indexOf(ContainerControlClientID) > -1)
            {
                    elem[i].checked=false;
            }
        }
        function BtnEnabled(rbtClientID,btnClientID,ContainerClientID)
        {
//            var rbtlist=document.getElementsByID("rbtDocWF");
            var elem=rbtClientID.form.elements;
            var j=0;  
            for(i=0;i<elem.length;i++)
            {
               if(elem[i].type=="radio"&& elem[i].checked==true )
               {
                  j=i+1;
                  elem[j].enabled=true;
               }
            }
        }