
function PopulateCarousel() {
    var $carousel = $("#pictures")
    $carousel.empty()
    for (var i = 0; i < window.picFiles.length; i += 4) {
        var item = document.createElement('div')
        item.classList.add('item')
        if (i == 0) {
            item.classList.add('active')
        }
        var row = document.createElement('div')
        row.classList.add('row')

        var col1 = document.createElement('div')
        col1.classList.add('col-sm-3', 'thumbnail', 'img-wrap')
        col1.innerHTML = `<img src="${window.picFiles[i]}" alt="Image" style=" height: 200px; overflow: hidden;" class="img-responsive"/><a href="#myCarousel" class="close" onClick="Delete(this)"   link-number="${i}">&times;</a>`
        row.appendChild(col1)

        if (window.picFiles[i + 1]) {
            var col2 = document.createElement('div')
            col2.classList.add('col-sm-3', 'thumbnail', 'img-wrap')
            col2.innerHTML = `<img src="${window.picFiles[i + 1]}" alt="Image" style=" height: 200px; overflow: hidden;"" class="img-responsive"/><a href="#myCarousel" class="close" onClick="Delete(this)"  link-number="${i + 1}">&times;</a>`
            row.appendChild(col2)

        }
        if (window.picFiles[i + 2]) {
            var col3 = document.createElement('div')
            col3.classList.add('col-sm-3', 'thumbnail', 'img-wrap')
            col3.innerHTML = `<img src="${window.picFiles[i + 2]}" alt="Image" style=" height: 200px; overflow: hidden;" class="img-responsive"/><a href="#myCarousel" class="close" onClick="Delete(this)"   link-number="${i + 2}">&times;</a>`
            row.appendChild(col3)
        }
        if (window.picFiles[i + 3]) {
            var col4 = document.createElement('div')
            col4.classList.add('col-sm-3', 'thumbnail', 'img-wrap')
            col4.innerHTML = `<img src="${window.picFiles[i + 3]}" alt="Image" style=" height: 200px; overflow: hidden;" class="img-responsive"/><a href="#myCarousel" class="close" style="left:2px !important;right:auto !important;" onClick="Delete(this)"  link-number="${i + 3}">&times;</a>`
            row.appendChild(col4)

        }
        item.append(row)
        $carousel.append(item)
    }
}


function ValidateApartmentInput() {
    var roomNo = $("#roomNo").val()
    var guestNo = $("#guestNo").val()
    var price = $("#price").val()
    var street = $("#street").val()
    var streetNo = $("#streetNo").val()
    var zipCode = $("#zipCode").val()
    var state = $("#state").val()
    var town = $("#town").val()

    var inputs = [roomNo, guestNo, price, street, streetNo, zipCode, state, town]
    var inputsNames = ["roomNo", "guestNo", "price", "street", "streetNo", "zipCode", "state", "town"]
    var regexes = [/^[1-9]([0-9]+)?$/, /^[1-9]([0-9]+)?$/, /^\d+[.]?(\d+)?$/, /^[_A-z]*((-|\s|\\)*[_A-z])*$/g, /^[a-zA-Z]?[1-9]([0-9]+)?[a-zA-Z/]?([0-9]+)?$/, /^[1-9]([0-9]+)?$/, /^[_A-z]*((-|\s|\\)*[_A-z])*$/g, /^[_A-z]*((-|\s|\\)*[_A-z])*$/g]
    var errors = ["Entered value has to be integer greater than 0", "Entered value has to be integer greater than 0", "Entered value has to be decimal value greater than 0",
        "Only letters and - \ are allowed", "Entered value has to be integer greater than 0", "Entered value has to be integer greater than 0", "Only letters and - \ are allowed"
        , "Only letters and - \ are allowed"]


    var isOk = Validate(inputs, inputsNames, regexes, errors)

    var dates = $("#date").datepicker().data('datepicker').selectedDates

    if (dates.length == 0) {
        isOk = false
        alert("Atleast one rent date has to be inputed.")
    }
    return isOk
}

function SendApartment(sendFunction) {
    if (ValidateApartmentInput()) {
        var coordinates = {}
        var address = `${$("#street").val()} ${$("#streetNo").val()},${$("#town").val()},${$("#zipCode").val()},${$("#state").val()}`
        address = encodeURIComponent(address)
        if (window.isMarkerUserInputed) {
            coordinates.Latitude = window.marker.getPosition().lat
            coordinates.Longitude = window.marker.getPosition().lng
            sendFunction(coordinates, address)
        } else {
            $.get('https://geocoder.api.here.com/6.2/geocode.json?app_id=fkzE4RgBilLoQPgzSnJb&app_code=ZM_tJ2tDZ0ZFwBFi98p-_g&searchtext=' + address, function (data) {
                if (data.Response.View[0].Result[0].Location) {
                    coordinates.Latitude = data.Response.View[0].Result[0].Location.DisplayPosition.Latitude
                    coordinates.Longitude = data.Response.View[0].Result[0].Location.DisplayPosition.Longitude
                }
                else {
                    coordinates.Latitude = 0
                    coordinates.Longitude = 0
                }

                sendFunction(coordinates, address)
            })
        }
       



    }
}

function PrepareGeneral(PictureChange,SendApartmentFunction) {
    var $container = $("#amenitiesCheckBoxes")
    $.get("/Apartment/GetAmenities", function (data) {
        window.amenities = []
        for (var i in data) {
            window.amenities.push(data[i])
        }
    for (var i = 0; i < data.length; i += 5) {
        var row = document.createElement('div')
        row.classList.add('row')

        var col1 = document.createElement('div')
        col1.classList.add('col-sm-1', 'form-check')
        col1.innerHTML = `<input type="checkbox" class="form-check-input" id="checkbox${data[i]}">
                                <label class="form-check-label" for="exampleCheck1">${data[i]}</label> `
        row.appendChild(col1)
        if (data[i + 1]) {
            var col2 = document.createElement('div')
            col2.classList.add('col-sm-1', 'form-check')
            col2.innerHTML = `<input type="checkbox" class="form-check-input" id="checkbox${data[i + 1]}">
                                <label class="form-check-label" for="exampleCheck1">${data[i + 1]}</label> `
            row.appendChild(col2)
        }
        if (data[i + 2]) {
            var col3 = document.createElement('div')
            col3.classList.add('col-sm-1', 'form-check')
            col3.innerHTML = `<input type="checkbox" class="form-check-input" id="checkbox${data[i + 2]}">
                                <label class="form-check-label" for="exampleCheck1">${data[i + 2]}</label> `
            row.appendChild(col3)
        }
        if (data[i + 3]) {
            var col4 = document.createElement('div')
            col4.classList.add('col-sm-1', 'form-check')
            col4.innerHTML = `<input type="checkbox" class="form-check-input" id="checkbox${data[i + 3]}">
                                <label class="form-check-label" for="exampleCheck1">${data[i + 3]}</label> `
            row.appendChild(col4)
        }
        if (data[i + 4]) {
            var col4 = document.createElement('div')
            col4.classList.add('col-sm-1', 'form-check')
            col4.innerHTML = `<input type="checkbox" class="form-check-input" id="checkbox${data[i + 4]}">
                                <label class="form-check-label" for="exampleCheck1">${data[i + 4]}</label> `
            row.appendChild(col4)
        }
        $container.append(row)
    }
});
$("#map").empty()
var map
var platform = new H.service.Platform({
    'app_id': 'fkzE4RgBilLoQPgzSnJb',
    'app_code': 'ZM_tJ2tDZ0ZFwBFi98p-_g',
    useHTTPS: true
});
var pixelRatio = window.devicePixelRatio || 1;
var defaultLayers = platform.createDefaultLayers({
    tileSize: pixelRatio === 1 ? 256 : 512,
    ppi: pixelRatio === 1 ? undefined : 320
});

map = new H.Map(
    document.getElementById('map'),
    defaultLayers.normal.map,
    { pixelRatio: pixelRatio });
var behavior = new H.mapevents.Behavior(new H.mapevents.MapEvents(map));

// Create the default UI components
var ui = H.ui.UI.createDefault(map, defaultLayers);
var marker = new H.map.Marker({ lat: 0, lng: 0 });

// Add the marker to the map:
map.addObject(marker);
window.map = map
window.marker = marker

    $("#date").datepicker({
        language: 'en',
        minDate: new Date()
    })
map.addEventListener('tap', function (evt) {
    var coord = map.screenToGeo(evt.currentPointer.viewportX,
        evt.currentPointer.viewportY);
    window.marker.setPosition({ lat:coord.lat, lng: coord.lng });
    window.isMarkerUserInputed = true

});

$("#buttonMap").click(function () {
    var street = $("#street").val()
    var streetNo = $("#streetNo").val()
    var town = $("#town").val()
    var state = $("#state").val()
    var zipCode = $("#zipCode").val()
    var string = `${street} ${streetNo},${town},${zipCode},${state}`
    string = encodeURIComponent(string)
    $.get('https://geocoder.api.here.com/6.2/geocode.json?app_id=fkzE4RgBilLoQPgzSnJb&app_code=ZM_tJ2tDZ0ZFwBFi98p-_g&searchtext=' + string, function (data) {
        if (data.Response.View[0].Result[0].Location.DisplayPosition) {
            window.map.setCenter({ lat: data.Response.View[0].Result[0].Location.DisplayPosition.Latitude, lng: data.Response.View[0].Result[0].Location.DisplayPosition.Longitude })
            window.map.setZoom(14)
            window.marker.setPosition({ lat: data.Response.View[0].Result[0].Location.DisplayPosition.Latitude, lng: data.Response.View[0].Result[0].Location.DisplayPosition.Longitude })
            window.isMarkerUserInputed = false
        } else {
            window.map.setCenter({ lat: 0, lng: 0})
            window.map.setZoom(1)
            window.marker.setPosition({ lat: 0, lng: 0 })
            window.isMarkerUserInputed = false
        }
    })
})

$('.clockpicker').clockpicker();

PictureChange()

$("#buttonSend").click(function () {
    SendApartment(SendApartmentFunction)
})
}

function Delete(elem) {
    var index = $(elem).attr("link-number") * 1
    window.picList.splice(index, 1)
    window.picFiles.splice(index, 1)
    PopulateCarousel()
    return false;
}
function PictureAddChangeApply() {
    $("#picture").change(function () {
        var reader = new FileReader()

        reader.onload = function (e) {
            window.picFiles.push(reader.result)
            PopulateCarousel()
        }
        if (this.files[0].type.split("/")[0] == "image") {
            reader.readAsDataURL(this.files[0])
            picList.push(this.files[0])
        } else {
            alert("File has to be an image");
        }


    })
}