function MakeElement(data) {
    var elem = document.createElement('div')
    elem.classList.add("col-sm-3");
    var click = document.createElement('a')

    click.setAttribute("elem", data.ID)
    click.setAttribute("data-toggle", "modal")
    click.setAttribute("data-target", "#previewModal")
    click.setAttribute("style", "cursor:pointer;text-decoration:none")
    click.setAttribute("onClick", "ShowPreview(this.getAttribute(\'elem\'))")
    var elemPanel = document.createElement('div');
    elemPanel.classList.add('panel', 'panel-primary');
    var elemHeading = document.createElement('div');
    elemHeading.classList.add('panel-heading');
    var address = data.Address.split(',');
    elemHeading.innerText = address[0] + ", " + address[1];
    elemPanel.appendChild(elemHeading);

    var elemBody = document.createElement('div')
    elemBody.classList.add('panel-body');
    var elemContent = document.createElement('p')
    elemContent.classList.add('lead', 'text-left');
    if (sessionStorage.currentRole && (sessionStorage.currentRole.toLowerCase() == "admin" || sessionStorage.currentRole.toLowerCase() == "host")) {
        elemContent.innerText =
            `
                      ${(data.Type == 'Whole') ? "Whole apartment" : "Room"}
                      ${(data.Type == 'Whole') ? ((data.RoomNumber > 1) ? data.RoomNumber + " Rooms" : data.RoomNumber + " Room") : ""}
                      ${data.GuestNumber} Guests
                      ${data.Status}
                      `;
    } else {
        elemContent.innerText =
            `
                      ${(data.Type == 'Whole') ? "Whole apartment" : "Room"}
                      ${(data.Type == 'Whole') ? ((data.RoomNumber > 1) ? data.RoomNumber + " Rooms" : data.RoomNumber + " Room") : ""}
                      ${data.GuestNumber} Guests
                      `;
    }
   
    elemBody.appendChild(elemContent);
    click.appendChild(elemBody);
    elemPanel.appendChild(click);

    var elemFooter = document.createElement('div')
    elemFooter.classList.add('panel-footer', 'container-fluid');
    var panelRow = document.createElement('div')
    panelRow.classList.add('row')
    var elemPriceCol = document.createElement('div')
    elemPriceCol.classList.add('col-sm-12')
    var elemPrice = document.createElement('p')
    elemPrice.classList.add('text-right','lead');
    elemPrice.innerText = `${data.Price.toFixed(2)} \u20AC/night`;
    elemPriceCol.appendChild(elemPrice);
    panelRow.appendChild(elemPriceCol)
    elemFooter.appendChild(panelRow)
    if (sessionStorage.currentRole && (sessionStorage.currentRole.toLowerCase() == "admin" || sessionStorage.currentRole.toLowerCase() == "host")) {
        var buttonsRow = document.createElement('div')
        buttonsRow.classList.add('row')
        var buttonDeleteCol = document.createElement('div')
        var buttonCol = document.createElement('div')
        buttonCol.classList.add('col-sm-12');
        //buttonDeleteCol.classList.add('col-sm-3')
        var buttonDelete = document.createElement('button')
        buttonDelete.classList.add('btn', 'btn-danger')
        buttonDelete.setAttribute('onClick', `DeleteApartment(${data.ID})`)
        buttonDelete.innerText = "Delete"
        buttonCol.appendChild(buttonDelete)
       // buttonsRow.appendChild(buttonDeleteCol)
        //var buttonUpdateCol = document.createElement('div')
        //buttonUpdateCol.classList.add('col-sm-3')
        var buttonUpdate = document.createElement('a')
        buttonUpdate.classList.add('btn', 'btn-primary')
        buttonUpdate.setAttribute('href', `UpdateApartment.html?id=${data.ID}`)
        buttonUpdate.innerText = "Update"
        buttonCol.appendChild(buttonUpdate)
       // buttonsRow.appendChild(buttonUpdateCol)

        //var buttonCommentCol = document.createElement('div')
        //buttonCommentCol.classList.add('col-sm-3')
     
   
        var buttonComment = document.createElement('button')
        buttonComment.classList.add('btn', 'btn-primary')
        buttonComment.setAttribute('onClick',`ShowComments(${data.ID})`)
        buttonComment.innerText = "Comments"
        buttonCol.appendChild(buttonComment)
       // buttonsRow.appendChild(buttonCommentCol)
        if (sessionStorage.currentRole.toLowerCase() == "admin" && data.Status == "Inactive") {
            //var buttonApproveCol = document.createElement('div')
            //buttonApproveCol.classList.add('col-sm-3')
            var buttonApprove = document.createElement('button')
            buttonApprove.classList.add('btn', 'btn-success')
            buttonApprove.setAttribute('onclick', `ApproveApartment(${data.ID})`)
            buttonApprove.innerText = "Approve"
            buttonCol.appendChild(buttonApprove)
           // buttonsRow.appendChild(buttonCommentCol)
        }
        buttonsRow.appendChild(buttonCol)
        elemFooter.appendChild(buttonsRow)
    }
  
    elemPanel.appendChild(elemFooter);
    
    elem.appendChild(elemPanel);

    return elem;

}

function MakeReservation(data) {
    var elem = document.createElement('div')
    elem.classList.add("col-sm-3");
    var click = document.createElement('a')

    click.setAttribute("elem", data.Apartment.ID)
    click.setAttribute("data-toggle", "modal")
    click.setAttribute("data-target", "#previewModal")
    click.setAttribute("style", "cursor:pointer;text-decoration:none")
    click.setAttribute("onClick", "ShowPreview(this.getAttribute(\'elem\'))")
    var elemPanel = document.createElement('div');
    elemPanel.classList.add('panel', 'panel-primary');
    var elemHeading = document.createElement('div');
    elemHeading.classList.add('panel-heading');
    var address = data.Apartment.Address.split(',');
    elemHeading.innerText = address[0] + ", " + address[1];
    elemPanel.appendChild(elemHeading);

    var elemBody = document.createElement('div')
    elemBody.classList.add('panel-body');
    var row1 = document.createElement('div')
    row1.classList.add('row')
    var col1 = document.createElement('div')
    col1.classList.add('col-sm-5')
    col1.innerHTML = `<p>Guest<br/>${data.GuestUsername}</p> `
    var col2 = document.createElement('div')
    col2.classList.add('col-sm-5')
    col2.innerHTML = `<p>No. Days<br/>${data.DaysNumber}</p> `
    row1.appendChild(col1)
    row1.appendChild(col2)
    var row2 = document.createElement('div')
    row2.classList.add('row')
    var col3 = document.createElement('div')
    col3.classList.add('col-sm-5')
    col3.innerHTML = `<p>Starting date<br/>${new Date(data.StartDate).toLocaleDateString()}</p> `
    var col4 = document.createElement('div')
    col4.classList.add('col-sm-5')
    col4.innerHTML = `<p>Price<br/>${data.Price.toFixed(2)} \u20AC</p> `
    row2.appendChild(col3)
    row2.appendChild(col4)
    var row3 = document.createElement('div')
    row3.classList.add('row')
    var col5 = document.createElement('div')
    col5.classList.add('col-sm-5')
    col5.innerHTML = `<p>State<br/>${data.State}</p> `
    row3.appendChild(col5)
    elemBody.appendChild(row1)
    elemBody.appendChild(row2)
    elemBody.appendChild(row3)
    
    click.appendChild(elemBody);
    elemPanel.appendChild(click);

    var elemFooter = document.createElement('div')
    elemFooter.classList.add('panel-footer', 'container-fluid');
    var panelRow = document.createElement('div')
    panelRow.classList.add('row')
    
    
    elemFooter.appendChild(panelRow)
    if (sessionStorage.currentRole && (sessionStorage.currentRole.toLowerCase() == "guest")) {
        var buttonsRow = document.createElement('div')
        buttonsRow.classList.add('row')
        var buttonCol = document.createElement('div')
        buttonCol.classList.add('col-sm-12');
        if (data.State == "Created" || data.State == "Accepted") {
            var buttonDelete = document.createElement('button')
            buttonDelete.classList.add('btn', 'btn-danger')
            buttonDelete.setAttribute('onClick', `CancelReservation(${data.ID})`)
            buttonDelete.innerText = "Cancel"
            buttonCol.appendChild(buttonDelete)
        }
        else if ((data.State == "Finished" || data.State == "Declined") && !data.CommentSetted) {
            var buttonUpdate = document.createElement('button')
            buttonUpdate.classList.add('btn', 'btn-primary')
            buttonUpdate.setAttribute('onClick', `AddCommentDialog(${data.ID})`)
            buttonUpdate.innerText = "Add Comment"
            buttonCol.appendChild(buttonUpdate)
        }

        buttonsRow.appendChild(buttonCol)
        elemFooter.appendChild(buttonsRow)
    }
    else if (sessionStorage.currentRole && (sessionStorage.currentRole.toLowerCase() == "host")) {
        var buttonsRow = document.createElement('div')
        buttonsRow.classList.add('row')
        var buttonCol = document.createElement('div')
        buttonCol.classList.add('col-sm-12');
        if (data.State == "Created") {
            var buttonApprove = document.createElement('button')
            buttonApprove.classList.add('btn', 'btn-success')
            buttonApprove.setAttribute('onclick', `AcceptReservation(${data.ID})`)
            buttonApprove.innerText = "Accept"
            buttonCol.appendChild(buttonApprove)
            var buttonDecline = document.createElement('button')
            buttonDecline.classList.add('btn', 'btn-danger')
            buttonDecline.setAttribute('onclick', `DeclineReservation(${data.ID})`)
            buttonDecline.innerText = "Decline"
            buttonCol.appendChild(buttonDecline)
            buttonsRow.appendChild(buttonCol)
            elemFooter.appendChild(buttonsRow)
        }
        if (data.State == 'Accepted') {
            var buttonDecline = document.createElement('button')
            buttonDecline.classList.add('btn', 'btn-danger')
            buttonDecline.setAttribute('onclick', `DeclineReservation(${data.ID})`)
            buttonDecline.innerText = "Decline"
            buttonCol.appendChild(buttonDecline)
            var buttonFinish = document.createElement('button')
            buttonFinish.classList.add('btn', 'btn-success')
            buttonFinish.setAttribute('onclick', `FinishReservation(${data.ID})`)
            buttonFinish.innerText = "Finish"
            buttonCol.appendChild(buttonFinish)
        }
        buttonsRow.appendChild(buttonCol)
        elemFooter.appendChild(buttonsRow)
    }

    elemPanel.appendChild(elemFooter);

    elem.appendChild(elemPanel);

    return elem;

}
function MakeAmenity(data) {
    var item = document.createElement('div')
    item.classList.add('col-sm-2')
    item.innerHTML = `<p>${data}</p>`
    return item
}
function ShowPreview(id) {
    $.ajax({
        url: "/Apartment/GetApartment?id=" + id,
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + sessionStorage.token
        },
        success: function (data) {
            window.currentItem = data
            var $imageContainer = $(".carousel-inner")
            $imageContainer.empty()
            for (var i = 0; i < data.Images.length; i++) {
                var item = document.createElement('div')
                item.classList.add('item')
                if (i == 0) {
                    item.classList.add('active')
                }

                var image = document.createElement('img')
                image.setAttribute('src', data.Images[i].Path)
                image.classList.add('img-responsive')
                image.setAttribute('style', 'width: 100%;height:500px;')
                item.appendChild(image)
                $imageContainer.append(item)
            }
            $("#type").html(`<p>Apartment Type<br/><span class="lead">${data.Type}</span></p>`)
            $("#price").html(`<p>Price<br/><span class="lead">${data.Price.toFixed(2)} \u20AC/night</span></p>`)
            $("#roomNo").html(`<p>Room number<br/><span class="lead">${data.RoomNumber}</span></p>`)
            $("#guestNo").html(`<p>Guest number<br/><span class="lead">${data.GuestNumber}</span></p>`)

            $("#checkIn").html(`<p>Check In<br/><span class="lead">${data.CheckInTime}</span></p>`)
            $("#checkOut").html(`<p>Check Out<br/><span class="lead">${data.CheckOutTime}</span></p>`)
            $("#address").html(`<p>Address<br/><span class="lead">${data.Location.Address.split(',').join(' ')}</span>`)

            setTimeout(function () {
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
                var marker = new H.map.Marker({ lat: data.Location.Latitude, lng: data.Location.Longitude });
                var ui = H.ui.UI.createDefault(map, defaultLayers);

                // Add the marker to the map:
                map.addObject(marker);
                map.setCenter({ lat: data.Location.Latitude, lng: data.Location.Longitude })
                map.setZoom(14)

            }, 100);
            var $amenities = $("#amenitiesLabel")
            $("#amenitiesLabel").nextAll('div').remove()
            for (var i = 0; i < data.Amenities.length; i = i + 6) {
                var row = document.createElement('div')
                row.classList.add('row')

                if (data.Amenities[i])
                    row.appendChild(MakeAmenity(data.Amenities[i]))
                if (data.Amenities[i + 1])
                    row.appendChild(MakeAmenity(data.Amenities[i + 1]))
                if (data.Amenities[i + 2])
                    row.appendChild(MakeAmenity(data.Amenities[i + 2]))
                if (data.Amenities[i + 3])
                    row.appendChild(MakeAmenity(data.Amenities[i + 3]))
                if (data.Amenities[i + 4])
                    row.appendChild(MakeAmenity(data.Amenities[i + 4]))
                if (data.Amenities[i + 5])
                    row.appendChild(MakeAmenity(data.Amenities[i + 5]))

                $amenities.after(row)
            }
            $("#previewComments").empty()
            $("#previewCommentsHeading").html(`<span class="lead">Comments ${data.Comments.length}</span>`)
            for (var a of data.Comments) {
                $("#previewComments").append(`
<div class="row">
<div class="col-sm-4">
<p>${a.GuestUsername} Rate ${a.Rate} -</p>
</div>
<div class="row">
<div class="col-sm-8 col-sm-offset-1">
<p>${a.Text}<p>
</div>
</div>
</div>

`)
            }
        }
        })

        
}

function ShowData(data, $itemContainer) {
    $itemContainer.empty()
    for (var i = 0; i < data.length; i += 4) {
        var row = document.createElement('div');
        row.classList.add('row');
        if (data[i])
            row.appendChild(MakeElement(data[i]));
        if (data[i + 1])
            row.appendChild(MakeElement(data[i + 1]));
        if (data[i + 2])
            row.appendChild(MakeElement(data[i + 2]));
        if (data[i + 3])
            row.appendChild(MakeElement(data[i + 3]));

        $itemContainer.append(row);
    }
}

function ShowReservations(data, $itemContainer) {
    $itemContainer.empty()
    for (var i = 0; i < data.length; i += 4) {
        var row = document.createElement('div');
        row.classList.add('row');
        if (data[i])
            row.appendChild(MakeReservation(data[i]));
        if (data[i + 1])
            row.appendChild(MakeReservation(data[i + 1]));
        if (data[i + 2])
            row.appendChild(MakeReservation(data[i + 2]));
        if (data[i + 3])
            row.appendChild(MakeReservation(data[i + 3]));

        $itemContainer.append(row);
    }
}

function ShowFullAmenitites(data, $container) {
    $container.empty();
    var holder = document.createElement('div')
    holder.classList.add('table-responsive')
    var table = document.createElement('table')
    table.classList.add('table')
    var tableHead = document.createElement('thead')
    tableHead.innerHTML = `<tr><td>Amenity</td><td>&nbsp;</td></tr>`
    table.appendChild(tableHead)
    var tableBody = document.createElement('tbody')

    for (var a of data) {
        var item = document.createElement('tr')
        item.innerHTML = `<td>${a.Name}</td><td><button class="btn btn-danger glyphicon glyphicon-trash" onClick="DeleteAmenity(${a.ID})"></button></td>`
        tableBody.appendChild(item)
    }
    table.appendChild(tableBody)
    holder.appendChild(table)
    $container.append(holder)

}

function ShowAllUsers(data, $container) {
    $container.empty();
    var holder = document.createElement('div')
    holder.classList.add('table-responsive')
    var table = document.createElement('table')
    table.classList.add('table')
    var tableHead = document.createElement('thead')
    tableHead.innerHTML = `<tr><td>Userame</td><td>Name</td><td>Last name</td><td>Gender</td><td>Role</td><td>&nbsp;</td></tr>`
    table.appendChild(tableHead)
    var tableBody = document.createElement('tbody')

    for (var a of data) {
        var item = document.createElement('tr')
        if (a.Role == "Admin")
            item.innerHTML = `<td>${a.Username}</td><td>${a.Name}</td><td>${a.LastName}</td><td>${a.Gender}</td><td>${a.Role}</td><td>&nbsp;</td>`
        else {
            if (a.Blocked)
                item.innerHTML = `<td>${a.Username}</td><td>${a.Name}</td><td>${a.LastName}</td><td>${a.Gender}</td><td>${a.Role}</td><td><button class="btn btn-primary" onClick="UnblockUser('${a.Username}')">Unblock</button></td>`
            else
                item.innerHTML = `<td>${a.Username}</td><td>${a.Name}</td><td>${a.LastName}</td><td>${a.Gender}</td><td>${a.Role}</td><td><button class="btn btn-danger" onClick="BlockUser('${a.Username}')">Block</button></td>`
            
        }
        tableBody.appendChild(item)
    }
    table.appendChild(tableBody)
    holder.appendChild(table)
    $container.append(holder)

}
