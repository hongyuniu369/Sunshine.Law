var setAddress = function (bindValue, valueField) {
    $.ajaxSettings.async = false;
    var province = $('#Province').combobox({
        url: '/api/msg/region/GetByPid/0',
        method: "Get",
        valueField: 'RegionID', //值字段
        textField: 'RegionName', //显示的字段
        editable: false,
        onSelect: function (record) {
            $.get('/api/msg/region/GetByPid/' + record.RegionID, function (data) {
                city.combobox("clear").combobox('loadData', data);
                district.combobox("clear")
            }, 'json');
            getValue();
        }
    });

    var city = $('#City').combobox({
        valueField: 'RegionID', //值字段
        textField: 'RegionName', //显示的字段
        editable: false,
        onSelect: function (record) {
            $.get('/api/msg/region/GetByPid/' + record.RegionID, function (data) {
                district.combobox("clear").combobox('loadData', data);
            }, 'json');
            getValue();
        }
    });

    var district = $('#District').combobox({
        valueField: 'RegionID', //值字段
        textField: 'RegionName', //显示的字段
        editable: false,
        onSelect: function (newValue, oldValue) {
            getValue();
        }
    });

    var getValue = function () {
        var selvalue = province.combobox("getText") + " " + city.combobox("getText") + " " + district.combobox("getText");
        viewModel.pageData.form.Address(selvalue);
    }

    var initValue = function () {
        if (bindValue) {
            var arrValue = bindValue.split(' ');
            if (arrValue[0])
                setSelectValue(province, arrValue[0]);
            if (arrValue[1])
                setSelectValue(city, arrValue[1]);
            if (arrValue[2])
                setSelectValue(district, arrValue[2]);
        }
        else
            $('#Province').combobox("select", 684);//初始化为河北省
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

    initValue();
}