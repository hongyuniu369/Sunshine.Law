var setAddress = function (view, valueField) {
    var bindValue = view.form.Address();
    $.ajaxSettings.async = false;
    var city = $('#City').combobox({
        url: '/api/msg/region/GetByPid/684',
        method: "Get",
        valueField: 'RegionID', //值字段
        textField: 'RegionName', //显示的字段
        editable: false,
        onSelect: function (record) {
            getValue();
        }
    });


    var getValue = function () {
        var selvalue = city.combobox("getText");
        view.form.Address(selvalue);
    }

    var initValue = function () {
        if (bindValue) {
            var arrValue = bindValue.split(' ');
            if (arrValue[1])
                setSelectValue(city, arrValue[1]);
        }
        else
            $('#City').combobox("select", 685);//初始化为河北省
    }
    var setSelectValue = function (obj, text) {//根据text选中select相应item
        var data = obj.combobox("getData");
        var selectedValue = null;
        try {
            selectedValue = $.grep(data, function (item) {
                return item.RegionName == text;
            })[0].RegionID;
        } catch (e) { }
        if (selectedValue) {
            obj.combobox("select", selectedValue);
        }
        else {
            obj.combobox("setText", text)
        }
    }

    //initValue();
}