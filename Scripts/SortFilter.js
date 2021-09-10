
function clone(obj) {
    var copy;

    // Handle the 3 simple types, and null or undefined
    if (null == obj || "object" != typeof obj) return obj;

    // Handle Date
    if (obj instanceof Date) {
        copy = new Date();
        copy.setTime(obj.getTime());
        return copy;
    }

    // Handle Array
    if (obj instanceof Array) {
        copy = [];
        for (var i = 0, len = obj.length; i < len; i++) {
            copy[i] = clone(obj[i]);
        }
        return copy;
    }

    // Handle Object
    if (obj instanceof Object) {
        copy = {};
        for (var attr in obj) {
            if (obj.hasOwnProperty(attr)) copy[attr] = clone(obj[attr]);
        }
        return copy;
    }

    throw new Error("Unable to copy obj! Its type isn't supported.");
}

function Filter(filterObj, data) {
    return data.filter(item => {
        for (var amenity of filterObj.Amenities) {
            if (item.Amenities == null) {
                return false
            }
            else if (!item.Amenities.includes(amenity))
                return false
        }
        if (filterObj.TypeFilter != "All")
            if (item.Type != filterObj.TypeFilter)
                return false
        if (filterObj.StatusFilter != "" && item.Status != filterObj.StatusFilter)
            return false

        return true
    })
}

function FilterApartment(data) {
   
    var filterObj = {}
    filterObj.Amenities = []
    $('input[name="ApartmentFilterAmenity"]:checked').each(function () {
        filterObj.Amenities.push($(this).attr('amenity'))
    })
    filterObj.TypeFilter = $("#typeFilter").children("option:selected").val()
    
    if ($("#statusFilter").length != 0)
        filterObj.StatusFilter = $("#statusFilter").children("option:selected").val()
    else
        filterObj.StatusFilter = ""
    window.currentApartmentFilter = clone(filterObj)

    return Filter(filterObj,data)
    
}


function FilterReservation(filterObj, data) {
    return data.filter(item => {
        
        if (filterObj.StateFilter != "All")
            if (item.State != filterObj.StateFilter)
                return false
       

        return true
    })
}

function FilterReservationMain(data) {

    var filterObj = {}
  
    filterObj.StateFilter = $("#stateFilter").val()
    window.currentReservationFilter = clone(filterObj)
    return FilterReservation(filterObj, data)

}

function filterOnClick() {
    window.previewReservations = clone(FilterReservationMain(window.allReservations))
    var container = $("#items")
    container.empty()
    ShowReservations(window.previewReservations, container)
}



function SortApartment(data) {
    var sortOrder = $("#priceSort").children("option:selected").val()
    if (sortOrder == 'Asc') {
        data.sort(function (a, b) {
            return a.Price - b.Price
        })
    }
    else {
        data.sort(function (a, b) {
            return b.Price - a.Price
        })
    }
}

function SearchApartment() {
    var searchObject = {}
    var checkIn = $('#checkInPicker').datepicker().data('datepicker').selectedDates
    if (checkIn.length != 0)
        searchObject.CheckInDate = checkIn[0]
    var checkOut = $('#checkOutPicker').datepicker().data('datepicker').selectedDates
    if (checkOut.length != 0)
        searchObject.CheckOutDate = checkOut[0]

    searchObject.Town = $("#townSearch").val()
    searchObject.State = $("#stateSearch").val()
    searchObject.MinPrice = $("#minPriceSearch").val()
    searchObject.MaxPrice = $("#maxPriceSearch").val()
    searchObject.RoomMin = $("#roomMinSearch").val()
    searchObject.RoomMax = $("#roomMaxSearch").val()
    searchObject.GuestNo = $("#guestNoSearch").val()
    window.currentApartmentSearch = clone(searchObject)
    $.ajax({
        url: '/Apartment/Search',
        method: 'POST',
        data: JSON.stringify(searchObject),
        contentType: "application/json",
        dataType: "json",
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.token
        },
        success: function (data) {
            window.allApartments = clone(data)
            if (window.currentApartmentFilter) {
                var items = filter(window.currentApartmentFilter, data)
                SortApartment(items)
                window.previewApartments = clone(items)
            }
            else {
                var items = data
                SortApartment(data)
                window.previewApartments = clone(items)
            }
            ShowData(window.previewApartments, $("#items"))
        }
    })
}

function SearchUsers() {
    var searchObject = {}
   

    searchObject.Username = $("#usernameSearch").val()
    searchObject.Role = $("#roleSearch").val()
    searchObject.Gender = $("#genderSearch").val()
   
    window.currentUserSearch = clone(searchObject)
    $.ajax({
        url: '/Account/Search',
        method: 'POST',
        data: JSON.stringify(searchObject),
        contentType: "application/json",
        dataType: "json",
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.token
        },
        success: function (data) {           
            ShowAllUsers(data, $("#items"))
        }
    })
}

function SearchReservations() {
    var searchObject = {}
    searchObject.Username = $("#usernameSearch").val()
    searchObject.Name = $("#nameSearch").val()
    searchObject.LastName = $("#lastNameSearch").val()
    searchObject.Gender = $("#genderSearch").val()
    
    window.currentReservationSearch = clone(searchObject)
    $.ajax({
        url: '/Reservation/Search',
        method: 'POST',
        data: JSON.stringify(searchObject),
        contentType: "application/json",
        dataType: "json",
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.token
        },
        success: function (data) {
            window.allReservations = clone(data)
            if (window.currentReservationFilter) {
                var items = filter(window.currentReservationFilter, data)
                SortApartment(items)
                window.previewReservations = clone(items)
            }
            else {
                var items = data
                SortApartment(data)
                window.previewReservations = clone(items)
            }
            ShowReservations(window.previewReservations, $("#items"))
        }
    })
}