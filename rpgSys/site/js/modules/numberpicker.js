/* 
    v0.2
    add Limiter control
    if u put <input type='hidden' id='npLimiter' value='$LIMIT'/>
    then all pickers will limited by npLimiter
    for max

    v0.3
    custom initialization
*/
var NumberPickerControl = {
    Create: function () {
        for (var i = 0; i < $('.picker').length; i++) {
            $('.picker')[i].innerHTML = npCreatePicker($('.picker')[i].attributes.selectr.value);
        }
        this.SetLimit(this.Limit);
    },
    Colsize:0,
    Init: function () {
        for (var i = 0; i < $('.picker').length; i++) {
            $('.picker')[i].innerHTML = npCreatePicker($('.picker')[i].attributes.selectr.value);
        }
    },
    SetLimit: function (limit) {
        if ($('#npLimiter').val() != undefined) {
            return $('#npLimiter').val(limit);
        }
    },
    Limit: 0
}

$(document).ready(function () {
    NumberPickerControl.Create();
});

function npCustomInit() {
    for (var i = 0; i < $('.picker').length; i++) {
        $('.picker')[i].innerHTML = npCreatePicker($('.picker')[i].attributes.selectr.value);
    }
}

function npCustomInit(collg){
    for (var i = 0; i < $('.picker').length; i++) {
        $('.picker')[i].innerHTML = npCreatePicker($('.picker')[i].attributes.selectr.value,collg);
    }
}

function npCreatePicker(name) {
    var html="<div class='input-group'><span class='input-group-btn'><button type='button' onclick=\"npPick('"+name+"', '-');\" class='btn btn-danger btn-number'><span class='glyphicon glyphicon-minus'>";
    html+="</span></button></span><input type='text' id='"+name+"Text' onchange=\"npValid('"+name+"');\" class='form-control' value='0' style='text-align:center'><input type='hidden' pickervalue='yes' id='"+name+"Value' value='0' />";
    html+= "<span class='input-group-btn'><button type='button' onclick=\"npPick('" + name + "', '+');\" class='btn btn-success btn-number'><span class='glyphicon glyphicon-plus'></span></button></span></div>";
    return html;
}

function npCreatePicker(name,collg) {
    var html = "<div class='input-group col-lg-"+collg+"'><span class='input-group-btn'><button type='button' onclick=\"npPick('" + name + "', '-');\" class='btn btn-danger btn-number'><span class='glyphicon glyphicon-minus'>";
    html += "</span></button></span><input type='text' id='" + name + "Text' onchange=\"npValid('" + name + "');\" class='form-control' value='0' style='text-align:center'><input type='hidden' pickervalue='yes' id='" + name + "Value' value='0' />";
    html += "<span class='input-group-btn'><button type='button' onclick=\"npPick('" + name + "', '+');\" class='btn btn-success btn-number'><span class='glyphicon glyphicon-plus'></span></button></span></div>";
    return html;
}

function npLimiter(){
    if($('#npLimiter').val()!=undefined){
        return parseInt($('#npLimiter').val());
    }else{
        return -1;
    }
}

function npLimiterValue(plusminus) {
    if (plusminus == '+') {
        var limit = npLimiter();
        if (limit != -1) {
            var llimit = 0;
            for (var i = 0; i < $('input').length; i++) {
                if ($('input')[i].attributes['pickervalue'] != undefined) {
                    if (parseInt($('input')[i].value) > 0) {
                        llimit += parseInt($('input')[i].value);
                    }
                }
            }
            if (llimit < limit)
                return true;
            else
                return false;
        }
        else
            return true;
    }
    else
        return true;
}

function npPick(name, plusminus) {
    if (npLimiterValue(plusminus)) {
        inputname = '#' + name + 'Text';
        hiddenname = '#' + name + 'Value';
        if (plusminus == '+') {
            $(hiddenname).val(parseInt($(hiddenname).val()) + 1);
        } else if (plusminus == '-') {
            $(hiddenname).val(parseInt($(hiddenname).val()) - 1);
        }
        $(inputname).val($(hiddenname).val());
    }
}

function npValid(name) {
    inputname = '#' + name + 'Text';
    hiddenname = '#' + name + 'Value';
    $(inputname).val($(hiddenname).val());
}