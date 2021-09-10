function Validate(inputs,inputsNames,inputsRegex,inputsErrors) {
    var isOk = true
    for (var index in inputs) {
        var capitalisedName = inputsNames[index].charAt(0).toUpperCase() + inputsNames[index].slice(1)
        if (!CheckIfOk(inputs[index], $("#div" + capitalisedName), $("#" + inputsNames[index] + "Icon"), $("#" + inputsNames[index] + "Help"), inputsRegex[index], inputsErrors[index])){
            isOk = false;
        }
    }
    return isOk
}



function CheckIfOk(input, $inputDiv, $inputIcon, $inputText, regex, errorText) {
    if (input.trim() == "") {
        $inputDiv.addClass("has-error")
        $inputIcon.removeClass('validationIconOff')
        $inputText.text("Field is required")
        return false
    }
    if (regex) {
        if (regex.test(input.trim())) {
            $inputDiv.removeClass("has-error")
            $inputIcon.addClass('validationIconOff')
            $inputText.text("")
            return true
        } else {
            $inputDiv.addClass("has-error")
            $inputIcon.removeClass('validationIconOff')
            $inputText.text(errorText)
            return false
        }
    }
    else {
        $inputDiv.removeClass("has-error")
        $inputIcon.addClass('validationIconOff')
        $inputText.text("")
        return true
    } 
   
}