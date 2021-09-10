function ReservationSetup() {
$("#reservationConfirm").click(function () {
    data = {
        'ID': window.currentItem.ID,
        'Times': window.dateArray
    }
    $.ajax({
        url: '/Reservation/SetReservation',
        data: JSON.stringify(data),
        method: 'POST',
        contentType: 'application/JSON',
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.token
        },
        success: function (data) {
            $("#previewModal").modal('hide')
            $("#createReservation").modal('hide')
            alert('Reservation is successfully created');
        },
        error: function (jqXHR, error, errorThrown) {
            if (jqXHR.status && jqXHR.status == 400) {
                alert('Apartment is unavailable on selected dates');
            } else if (jqXHR.status && jqXHR.status == 403) {
                    alert("Your account has been blocked");
                    Logout()
                } else {
                    alert("Something went wrong");
                }
            

        }
        
    })
})
    $("#previewModal").on("show.bs.modal", function () {
       
        if (sessionStorage.currentView == "Apartments") {
            $("#previewFooter").empty()
            var button = document.createElement('button')
            button.classList.add('btn', 'btn-primary')
            button.setAttribute('data-toggle', 'modal')
            button.setAttribute('data-target', '#createReservation')
            button.setAttribute('data-dismiss', 'modal')
            button.textContent = 'Set reservation'
            $("#previewFooter").append(button)

        }
        

    })
$("#createReservation").on('shown.bs.modal', function () {
    $("#datePicker").datepicker().data('datepicker').destroy()
    $("#totalPrice").text('0 \u20AC')
    var availableDates = []
    var apartmentData = window.currentItem
    for (var i in apartmentData.AvailableDates)
        availableDates.push(new Date(apartmentData.AvailableDates[i]).getTime())
    window.avalableDates = availableDates

    $("#datePicker").datepicker({
        language: 'en',
        range: true,
        inline: true,
        toggleSelected: false,
        minDate : new Date(),
        onRenderCell: function (date, cellType) {
            if (cellType == 'day') {

                if (!window.avalableDates.includes(new Date(date).getTime())) {
                    return {
                        disabled: true
                    }
                }
            }
        },
        onSelect: function (formattedDate, date, inst) {
            if (date.length == 2) {

                var arr = GetDateArray(date[0].toISOString(), date[1].toISOString())
                var isOk = true
                for (var ind in arr) {
                    if (!window.avalableDates.includes(arr[ind])) {
                        $("#datePicker").datepicker().data('datepicker').clear()
                        isOk = false
                    }
                }
                if (isOk) {
                    var dateArray = []
                    for (var i in arr) {
                        dateArray.push(new Date(arr[i]).toJSON())
                    }
                    window.dateArray = []
                    for (var i in dateArray) {
                        window.dateArray.push(dateArray[i])
                    }
                    data = {
                        'ID': window.currentItem.ID,
                        'Times': dateArray
                    }

                    $("#reservationConfirm").prop('disabled', true)
                    $.ajax({
                        url: '/Reservation/GetPrice',
                        data: JSON.stringify(data),
                        method: 'POST',
                        contentType: 'application/JSON',
                        success: function (data) {
                            if (data != -1) {
                                $("#totalPrice").text(data.toFixed(2) +  '\u20AC')
                                $("#reservationConfirm").prop('disabled', false)
                            }
                        }
                    })
                }

            }
        }
    });

})
}
function GetDateArray(start, end) {
    var arr = new Array(), dt = new Date(start), dtEnd = new Date(end);
    while (dt <= dtEnd) {
        arr.push(new Date(dt).getTime());
        dt.setDate(dt.getDate() + 1);
    }
    return arr;
}