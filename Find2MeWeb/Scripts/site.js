

// Post Data
function PostMyData(data, method, SuccessFunction) {
  $.ajax({
    type: "POST",
    url: method,
    data: data,
    dataType: 'json',
    success: SuccessFunction,
    failure: function (response) {
      //location.href = 'Shared/Error';
      console.log(response.responseText);
    },
    error: function (response) {
      //location.href = 'Shared/Error';
      console.log(response.responseText);
    }
  });
}

// Get Data
function GetMyData(method, SuccessFunction) {
  $.ajax({
    type: "GET",
    url: method,
    dataType: 'json',
    success: SuccessFunction,
    failure: function (response) {
      //location.href = 'Shared/Error';
      console.log(response.responseText);
    },
    error: function (response) {
      //location.href = 'Shared/Error';
      console.log(response.responseText);
    }
  });
}
function DeleteItem(e) {
  // prevent the ancor tag from going to its specified link
  e.preventDefault();

  // Pop up confirm model
  $.confirm({
    title: 'Delete!',
    content: 'Are you sure you want to delete this record?',
    buttons: {
      confirm: function () {
        // if clicked on confirm button go to the specified link
        location.href = e.target.getAttribute('href');
      },
      close: function () {
        // else if clicked on close then just close the model
      }
    }
  });
}

// Validate Emails
function ValidateEmail(email) {
  debugger;
  var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
  return re.test(String(email).toLowerCase());
}

// Validate Phone No (11 Digits)
// Limit Phone entry
function LimitPhone(element, length) {
  var max_chars = length;

  if (element.value.length > max_chars) {
    element.value = element.value.substr(0, max_chars);
  }
}



