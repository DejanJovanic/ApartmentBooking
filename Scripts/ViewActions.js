function DeleteApartment(id) {
    $.ajax({
        url: '/Apartment/DeleteApartment?id=' + id,
        method: 'GET',
     
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.token
        },
        success: function (data) {
            RefreshApartments();
        },
        error: function (jqXHR, error, errorThrown) {
            if (jqXHR.status && jqXHR.status == 403) {
                alert("Your account has been blocked");
                Logout()
            } else {
                alert("Something went wrong");
            }
        }
    })
}

function RefreshApartments() {
    if (window.currentApartmentSearch) {
        $.ajax({
            url: '/Apartment/Search',
            method: 'POST',
            data: JSON.stringify(window.currentApartmentSearch),
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
    } else {
        $.ajax({
            url: "/Apartment/GetDisplays",
            method: "GET",
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
}


function RefreshReservations() {
    if (window.currentReservationSearch) {
        $.ajax({
            url: '/Reservation/Search',
            method: 'POST',
            data: JSON.stringify(window.currentApartmentSearch),
            contentType: "application/json",
            dataType: "json",
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.token
            },
            success: function (data) {
                window.allReservations = clone(data)
                if (window.currentReservationFilter) {
                    var items = filter(window.currentApartmentFilter, data)
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
    } else {
        $.ajax({
            url: "/Reservation/GetReservationsDisplays",
            method: "GET",
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
}

function BlockUser(name) {
    $.ajax({
        url: "/Account/BlockUser?username=" + name,
        method: "GET",
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.token
        },
        success: function (data) {

            RefreshUsers(data, $("#items"))
        }
    })
}

function UnblockUser(name) {
    $.ajax({
        url: "/Account/UnblockUser?username=" + name,
        method: "GET",
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.token
        },
        success: function (data) {

            RefreshUsers(data, $("#items"))
        }
    })
}

function RefreshUsers() {
    if (window.currentUserSearch) {
        $.ajax({
            url: '/Account/Search',
            method: 'POST',
            data: JSON.stringify(window.currentUserSearch),
            contentType: "application/json",
            dataType: "json",
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.token
            },
            success: function (data) {
               
                ShowAllUsers(data, $("#items"))
            }
        })
    } else {
        $.ajax({
            url: "/Account/GetAllUsers",
            method: "GET",
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.token
            },
            success: function (data) {
               

                ShowAllUsers(data, $("#items"))
            }
        })
    }
}

function ApproveApartment(id) {
    $.ajax({
        url: '/Apartment/ApproveApartment?id=' + id,
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.token
        },
        success: function (data) {
            RefreshApartments()
        }        
    })
}

function AcceptReservation(id) {
    $.ajax({
        url: '/Reservation/AcceptReservation?id=' + id,
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.token
        },
        success: function (data) {
            RefreshReservations()
        },
        error: function (jqXHR, error, errorThrown) {
            if (jqXHR.status && jqXHR.status == 403) {
                alert("Your account has been blocked");
                Logout()
            } else {
                alert("Something went wrong");
            }
        }
    })
}

function DeclineReservation(id) {
    $.ajax({
        url: '/Reservation/DeclineReservation?id=' + id,
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.token
        },
        success: function (data) {
            RefreshReservations()
        },
        error: function (jqXHR, error, errorThrown) {
            if (jqXHR.status && jqXHR.status == 403) {
                alert("Your account has been blocked");
                Logout()
            } else {
                alert("Something went wrong");
            }
        }
    })
}
function CancelReservation(id) {
    $.ajax({
        url: '/Reservation/CancelReservation?id=' + id,
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.token
        },
        success: function (data) {
            RefreshReservations()
        },
        error: function (jqXHR, error, errorThrown) {
            if (jqXHR.status && jqXHR.status == 403) {
                alert("Your account has been blocked");
                Logout()
            } else {
                alert("Something went wrong");
            }
        }
    })
}

function FinishReservation(id) {
    $.ajax({
        url: '/Reservation/FinishReservation?id=' + id,
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.token
        },
        success: function (data) {
            RefreshReservations()
        },
        error: function (jqXHR, error, errorThrown) {
            if (jqXHR.status && jqXHR.status == 403) {
                alert("Your account has been blocked");
                Logout()
            } else {
                alert("Something went wrong");
            }
        }
    })
}

function AddCommentDialog(id) {
    $("#rateInput").val('1')
    $("#commentInput").val('')
    $("#reservationId").val(id)
    $("#commentAddModal").modal('show')
}

function SubmitComment() {
    var comment = {}
    comment.ID = $("#reservationId").val()
    comment.Rate = $("#rateInput").val()
    comment.Text = $("#commentInput").val()
    $.ajax({
        url: "/Reservation/AddComment",
        method: "POST",
        data: JSON.stringify(comment),
        contentType: "application/json",
        dataType: "json",
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.token
        },
        success: function (data) {
            $("#commentAddModal").modal('hide')
            RefreshReservations()
        },
        error: function (jqXHR, error, errorThrown) {
            if (jqXHR.status && jqXHR.status == 403) {
                alert("Your account has been blocked");
                Logout()
            } else {
                alert("Something went wrong");
            }
        }
    })
}

function ShowComments(id) {
    GetComments(id)
    window.currentCommentId = id
    $("#commentAllModal").modal('show')
}

function GetComments(id) {
    $("#commentsContent").empty()
    $.ajax({
        url: '/Apartment/GetComments?id=' + id,
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.token
        },
        success: function (data) {
          
            for (var a of data) {
                if (a.Deleted) {
                    $("#commentsContent").append(`
<tr>
<td>${a.GuestUsername}</td>
<td>${a.Rate}</td>
<td>${a.Text}</td>
<td><span style="color:red;">Deleted</span></td>
</tr>
`)
                }
                else {
                    $("#commentsContent").append(`
<tr>
<td>${a.GuestUsername}</td>
<td>${a.Rate}</td>
<td>${a.Text}</td>
<td><button class="btn btn-danger glyphicon glyphicon-trash" onClick="DeleteComment(${a.ID})"></button></td>
</tr>
`)
                }
            }
        }

    })
}

function Logout() {
    sessionStorage.clear()
    window.location = "Index.html"
    return false;
}

function DeleteComment(id) {
    $.ajax({
        url: "/Apartment/DeleteComment?id=" + id,
        method: "GET",
        //data: JSON.stringify(comment),
        //contentType: "application/json",
        //dataType: "json",
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.token
        },
        success: function (data) {
            GetComments(window.currentCommentId)
           
        },
        error: function (jqXHR, error, errorThrown) {
            if (jqXHR.status && jqXHR.status == 403) {
                alert("Your account has been blocked");
                Logout()
            } else {
                alert("Something went wrong");
            }
        }
    })
}