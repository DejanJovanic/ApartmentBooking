var ApartmentsSearchBody
var ApartmentsFilterBody
var ApartmentsSortBody

var ReservationsSearchBody
var ReservationsFilterBody
var ReservationsSortBody

var AmenitiesSearchBody 
var AmenitiesFilterBody 
var AmenitiesSortBody 

var UsersSearchBody
var UsersFilterBody
var UsersSortBody 
function MakeApartmentSearchMain() {
    $("#SearchPanel").html(`
<div id="SearchPanelBody">                      
 <div class="container-fluid panel-body" style="margin:4px 0px 4px 0px">
                                <form>
                                    <div class="row">
                                        <div class="form-group col-sm-6">
                                            <label class="control-label" for="checkInPicker">Check in date</label>
                                            <input type="button" class="datepicker-here form-control" id="checkInPicker" />
                                        </div>
                                        <div class="form-group col-sm-6">
                                            <label class="control-label" for="checkOutPicker">Check out date</label>
                                            <input type="button" class="datepicker-here form-control" id="checkOutPicker" />
                                        </div>

                                    </div>
                                    <div class="row">
                                        <div class="form-group col-sm-6">
                                            <label class="control-label" for="townSearch">Town/City</label>
                                            <input type="text" id="townSearch" class="form-control" />
                                        </div>
                                        <div class="form-group col-sm-6">
                                            <label class="control-label" for="stateSearch">State/Country</label>
                                            <input type="text" id="stateSearch" class="form-control" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group col-sm-6">
                                            <label class="control-label" for="minPriceSearch">Min price</label>
                                            <input type="text" id="minPriceSearch" class="form-control" />
                                        </div>
                                        <div class="form-group col-sm-6">
                                            <label class="control-label" for="maxPriceSearch">Max price</label>
                                            <input type="text" id="maxPriceSearch" class="form-control" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group col-sm-6">
                                            <label class="control-label" for="roomMinSearch">Min rooms</label>
                                            <input type="text" id="roomMinSearch" class="form-control" />
                                        </div>
                                        <div class="form-group col-sm-6">
                                            <label class="control-label" for="roomMaxSearch">Max rooms</label>
                                            <input type="text" id="roomMaxSearch" class="form-control" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group col-sm-6">
                                            <label class="control-label" for="guestNoSearch">Number of guests</label>
                                            <input type="text" id="guestNoSearch" class="form-control" />
                                        </div>
                                        <div class="form-group col-sm-6">
                                            <label class="control-label" for="buttonSearch">&nbsp;</label>
                                            <button type="button" id="buttonSearch" class="btn btn-primary form-control">Search</button>
                                        </div>
                                    </div>
                                </form>
                            </div>
</div>

`)
}

function MakeApartmentFilterMain() {
    $("#FilterPanel").html(`
<div id="FilterPanelBody">

                            
 <div class="panel-body">
                                <div class="row container-fluid">
                                    <div class="row">
                                        <div class="col-sm-12" id="FilterPanelData">

                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <label for="typeFilter" class="control-label">Apartment type</label>
                                            <select class="form-control" id="typeFilter">
                                                <option value="All">All apartments</option>
                                                <option value="Whole">Whole apartments</option>
                                                <option value="Room">Rooms</option>
                                            </select>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-6" id="statusFilterContainer">

                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-6">&nbsp;</div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-6">
                                        <button id="buttonFilter" class="btn btn-primary">Filter</button>
                                    </div>
                                </div>
                            </div>
</div>
`)
}

function MakeSort(){
    $("#sortRow").html(`<div id="sortCol" class="col-sm-5 col-sm-offset-10">
                <div id="sortBody">
                <label for="priceSort">Price:</label>
                <select id="priceSort" onChange="SortOnChange()">
                    <option value="Asc">Ascending</option>
                    <option value="Desc">Descending</option>
                </select>
                </div>
            </div>

`)
    
}
function MakeSortHost() {
    $("#sortRow").html(`
            <div id="sortCol" class="col-sm-12">
            <div class="col-sm-3 col-sm-offset-3" >
            <a class="btn btn-primary" href="AddApartment.html"> Add apartment </a>
            </div>
            <div  class="col-sm-5 col-sm-offset-10">
                <div id="sortBody">
                <label>Price:</label>
                <select id="priceSort" onChange="SortOnChange()">
                    <option value="Asc">Ascending</option>
                    <option value="Desc">Descending</option>
                </select>
                </div>
            </div>
            </div>
             

`)
}

function MakeSortUser() {
    $("#sortRow").html(`
            <div id="sortCol" class="col-sm-12">
            <div class="col-sm-3 col-sm-offset-3" >
            <a class="btn btn-primary" href="AddHost.html"> Add Host </a>
            </div>
            
            </div>
             

`)
}

function MakeSortAmenity() {
    $("#sortRow").html(`
            <div id="sortCol" class="col-sm-12">
            <div class="col-sm-3 col-sm-offset-3" >
            <button class="btn btn-primary" data-toggle="modal" data-target="#amenitiesModal"> Add amenity </button>
            </div>
            </div>
             

`)
}


function SortOnChange() {
    if(sessionStorage.currentView == "Apartments") {
        SortApartment(window.previewApartments)
        ShowData(window.previewApartments, $("#items"))
    }
    else if (sessionStorage.currentView == "Reservations") {
        SortApartment(window.previewReservations)
        ShowReservations(window.previewReservations, $("#items"))
    }
}

function ChangeView(nextView,sortFunction) {

    switch (sessionStorage.currentView) {
        case "Apartments":
            ApartmentsSearchBody = $("#SearchPanelBody").detach()
            ApartmentsFilterBody = $("#FilterPanelBody").detach()
            ApartmentsSortBody = $("#sortCol").detach()
            $("#items").empty()
            $("#apartmentsActive").removeClass('active')
            break;
        case "Reservations":
            ReservationsSearchBody = $("#SearchPanelBody").detach()
            ReservationsFilterBody = $("#FilterPanelBody").detach()
            ReservationsSortBody = $("#sortCol").detach()
            $("#items").empty()
            $("#reservationsActive").removeClass('active')
            break;
        case "Amenities":
            AmenitiesSearchBody = $("#SearchPanelBody").detach()
            AmenitiesFilterBody = $("#FilterPanelBody").detach()
            AmenitiesSortBody = $("#sortCol").detach()
            $("#items").empty()
            $("#amenitiesActive").removeClass('active')
            break;
        case "Users":
            UsersSearchBody = $("#SearchPanelBody").detach()
            UsersFilterBody = $("#FilterPanelBody").detach()
            UsersSortBody = $("#sortCol").detach()
            $("#items").empty()
            $("#usersActive").removeClass('active')
            break;
    }

    switch (nextView) {
        case "Apartments":
            if (ApartmentsSearchBody) 
                ApartmentsSearchBody.appendTo("#SearchPanel")        
            else
                MakeApartmentSearchMain()
            if (ApartmentsFilterBody)
                ApartmentsFilterBody.appendTo("#FilterPanel")
            else
                MakeApartmentFilterMain()
            if (ApartmentsSortBody)
                ApartmentsSortBody.appendTo("#sortRow")
            else
                sortFunction()
           
            RefreshApartments()
            sessionStorage.currentView = "Apartments"
            $("#apartmentsActive").addClass('active')
            break;

             case "Reservations":
            if (ReservationsSearchBody)
                ReservationsSearchBody.appendTo("#SearchPanel")
            else
                MakeReservationSearchMain()

            if (ReservationsFilterBody)
                ReservationsFilterBody.appendTo("#FilterPanel")
            else
                MakeReservationFilterMain()

            if (ReservationsSortBody)
                ReservationsSortBody.appendTo("#sortRow")
            else
                MakeSort()

            RefreshReservations()

            sessionStorage.currentView = "Reservations"
            $("#reservationsActive").addClass('active')
            break;

        case "Amenities":
            if (AmenitiesSearchBody)
                AmenitiesSearchBody.appendTo("#SearchPanel")
            else
                MakeAmenitySearchMain()

            if (AmenitiesFilterBody)
                AmenitiesFilterBody.appendTo("#FilterPanel")
            else
                MakeAmenityFilterMain()

            if (AmenitiesSortBody)
                AmenitiesSortBody.appendTo("#sortRow")
            else
                MakeSortAmenity()

            MakeAmenitiesItemsBody()

            sessionStorage.currentView = "Amenities"
            $("#amenitiesActive").addClass('active')
            break;

        case "Users":
            if (UsersSearchBody)
                UsersSearchBody.appendTo("#SearchPanel")
            else
                MakeUserSearchMain()

            if (UsersFilterBody)
                UsersFilterBody.appendTo("#FilterPanel")
            else
                MakeUserFilterMain()

            if (UsersSortBody)
                UsersSortBody.appendTo("#sortRow")
            else
                MakeSortUser()

            RefreshUsers()

            sessionStorage.currentView = "Users"
            $("#usersActive").addClass('active')
            break;
    }
 
    
}

function MakeApartmentsItemsBody() {
    $.ajax({
        url: "/Apartment/GetDisplays",
        method: "GET",
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.token
        },
        success: function (data) {
           

            window.allApartments = clone(data)
            window.previewApartments = clone(data)
            SortApartment(window.previewApartments)
            $("#itemsHolder").html('<div class="container-fluid col-sm-9" id="items"></div>')
            ShowData(window.previewApartments, $("#items"));


        }
    })
}

function MakeReservationItemsBody() {
    $.ajax({
        url: "/Reservation/GetReservationsDisplays",
        method: "GET",
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.token
        },
        success: function (data) {


            window.allReservations = clone(data)
            window.previewReservations = clone(data)
            SortApartment(window.previewReservations)
            $("#itemsHolder").html('<div class="container-fluid col-sm-9" id="items"></div>')
            ShowReservations(window.previewReservations, $("#items"));


        }
    })
}

function MakeAmenitiesItemsBody() {
    $.ajax({
        url: "/Apartment/GetFullAmenities",
        method: "GET",
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.token
        },
        success: function (data) {


            $("#itemsHolder").html('<div class="container-fluid col-sm-9" id="items"></div>')
            ShowFullAmenitites(data, $("#items"));


        }
    })
}

function MakeUsersItemsBody() {
    $.ajax({
        url: "/Account/GetAllUsers",
        method: "GET",
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.token
        },
        success: function (data) {
            $("#itemsHolder").html('<div class="container-fluid col-sm-9" id="items"></div>')
            ShowAllUsers(data, $("#items"));


        }
    })
}

function MakeReservationFilterMain() {
    if (sessionStorage.currentRole && (sessionStorage.currentRole.toLowerCase() == "host" || sessionStorage.currentRole.toLowerCase() == "admin")) {
        $("#FilterPanel").html(`
<div id="FilterPanelBody">

                            
 <div class="panel-body">
                                <div class="row container-fluid">
                                      <div class="row">
                                        <div class="col-sm-6">
                                            <label class="control-label" for="stateFilter" >Reservation state</label>
                                            <select class="form-control" id="stateFilter">
                                                <option value="All">All</option>
                                                <option value="Accepted">Accepted</option>
                                                <option value="Declined">Declined</option>
                                                <option value="Finished">Finished</option>
                                                <option value="Created">Created</option>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                                
                                <div class="row">
                                    <div class="col-sm-6">&nbsp;</div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-6">
                                        <button id="buttonFilter" onClick="filterOnClick()" class="btn btn-primary">Filter</button>
                                    </div>
                                </div>
                            </div>
</div>
`)
    } else {
        $("#FilterPanel").html(`
<div id="FilterPanelBody">                      
 <div class="container-fluid panel-body" style="margin:4px 0px 4px 0px"> Filter option is currently unavailable </div></div>`)
    }
}

function MakeAmenityFilterMain() {
    $("#FilterPanel").html(`
<div id="FilterPanelBody">                      
 <div class="container-fluid panel-body" style="margin:4px 0px 4px 0px"> Filter option is currently unavailable </div></div>`)
}


function MakeUserFilterMain() {
    $("#FilterPanel").html(`
<div id="FilterPanelBody">                      
 <div class="container-fluid panel-body" style="margin:4px 0px 4px 0px"> Filter option is currently unavailable </div></div>`)
}

function MakeReservationSearchMain() {
    if (sessionStorage.currentRole && (sessionStorage.currentRole.toLowerCase() == "host" || sessionStorage.currentRole.toLowerCase() == "admin")) {
        $("#SearchPanel").html(`
<div id="SearchPanelBody">                      
 <div class="container-fluid panel-body" style="margin:4px 0px 4px 0px">
                                <form>
                                    <div class="row">
                                        <div class="form-group col-sm-6">
                                            <label class="control-label" for="usernameSearch">Username</label>
                                            <input type="text" class="form-control" id="usernameSearch" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group col-sm-6">
                                            <label class="control-label" for="nameSearch">Name</label>
                                            <input type="text" id="nameSearch" class="form-control" />
                                        </div>
                                        
                                    </div>
                                    <div class="row">
                                      <div class="form-group col-sm-6">
                                            <label class="control-label" for="lastNameSearch">Last name</label>
                                            <input type="text" id="lastNameSearch" class="form-control" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group col-sm-6">
                                         <label class="control-label" for="genderSearch">Gender</label>
                                        <select class="form-control" id="genderSearch">
                                            <option value="All">All genders</option>
                                            <option value="Male">Male</option>
                                            <option value="Female">Female</option>
                                        </select>
                                    </div>
                                    </div>
                                    
                                        <div class="form-group col-sm-6">
                                            <label class="control-label" for="buttonSearch">&nbsp;</label>
                                            <button type="button" id="buttonSearch" onClick="SearchReservations()" class="btn btn-primary form-control">Search</button>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>

`)
    } else {
        $("#SearchPanel").html(`
<div id="SearchPanelBody">                      
 <div class="container-fluid panel-body" style="margin:4px 0px 4px 0px"> Search option is currently unavailable </div></div>`)
    }

}

function MakeUserSearchMain() {
    $("#SearchPanel").html(`
<div id="SearchPanelBody">                      
 <div class="container-fluid panel-body" style="margin:4px 0px 4px 0px">
                                <form>
                                    <div class="row">
                                        <div class="form-group col-sm-6">
                                            <label class="control-label" for="usernameSearch">Username</label>
                                            <input type="text" class="form-control" id="usernameSearch" />
                                        </div>
                                    </div>                                       
   
                                    <div class="row">
                                        <div class="form-group col-sm-6">
                                         <label class="control-label" for="genderSearch">Gender</label>
                                        <select class="form-control" id="genderSearch">
                                            <option value="All">All genders</option>
                                            <option value="Male">Male</option>
                                            <option value="Female">Female</option>
                                        </select>
                                    </div>
                                    </div>
                                      <div class="row">
                                        <div class="form-group col-sm-6">
                                         <label class="control-label" for="genderSearch">Role</label>
                                        <select class="form-control" id="roleSearch">
                                            <option value="All">All genders</option>
                                            <option value="Guest">Guest</option>
                                            <option value="Host">Host</option>
                                            <option value="Admin">Admin</option>
                                        </select>
                                    </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group col-sm-6">
                                            <label class="control-label" for="buttonSearch">&nbsp;</label>
                                            <button type="button" id="buttonSearch" onClick="SearchUsers()" class="btn btn-primary form-control">Search</button>
                                        </div>
                                    </div>
                                    </div>
                                </form>
                            </div>
                        </div>

`)
}

function MakeAmenitySearchMain() {
    $("#SearchPanel").html(`
<div id="SearchPanelBody">                      
 <div class="container-fluid panel-body" style="margin:4px 0px 4px 0px"> Search option is currently unavailable </div></div>`)
}
function MakeApartmentStatusFilter() {
    // if (localStorage.user == "Admin" || localStorage.user == "Host") {
    $("#statusFilterContainer").append(`
         <label for="statusFilter" class="control-label" >Apartment status</label>
            <select class="form-control" id="statusFilter">
                <option value="All">All apartments</option>
                <option value="Active">Active apartments</option>
                <option value="Inactive">Inactive apartments</option>
            </select>`)


    //}
}
function MakeApartmentFilterAmenities() {
    $.get("/Apartment/GetAmenities", function (data) {

        for (var i = 0; i < data.length; i += 3) {
            var row = document.createElement('div')
            row.classList.add('row')
            var col1 = document.createElement('div')
            col1.classList.add('col-sm-3')
            col1.innerHTML = `
                            <label  for="filterCheckbox${data[i]}">${data[i]}</label>
                            <input type="checkbox" amenity="${data[i]}" class="form-check-input" name="ApartmentFilterAmenity"">`


            row.appendChild(col1)
            if (data[i + 1]) {
                var col2 = document.createElement('div')
                col2.classList.add('col-sm-3')
                col2.innerHTML = `
                             <label  for="filterCheckbox${data[i + 1]}">${data[i + 1]}</label>
                            <input type="checkbox" amenity="${data[i + 1]}" class="form-check-input" name="ApartmentFilterAmenity"">`


                row.appendChild(col2)

            }
            if (data[i + 2]) {
                var col3 = document.createElement('div')
                col3.classList.add('col-sm-3')
                col3.innerHTML = `
                             <label  for="filterCheckbox${data[i + 2]}">${data[i + 2]}</label>
                            <input type="checkbox" amenity="${data[i + 2]}" class="form-check-input" name="ApartmentFilterAmenity"">`


                row.appendChild(col3)

            }

            $("#FilterPanelData").append(row)
        }
    })
}

