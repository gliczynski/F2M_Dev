window.onload = (() => {
  // get the calculated ad end date and show in page
  adend.textContent = addDays(new Date(Date.now()), 30);
  $('#ad_end_date_server_value').val(addDays(new Date(Date.now()), 30));

  $('#AdStartDate').datepicker({
    autoclose: true
  });

  // CAlculateTOtalAdPRice
  $('#total_ad_price').html(CalculateTotalAdPrice());

  // Check if reward is checked or not
  let isCheckedReward = $('#IsReward').is(":checked") ? true : false;
  if (isCheckedReward) {
    setrReward.style.setProperty("display", "flex", "important");
    rewardAmount.style.setProperty("display", "flex", "important");
    $('#reward_price_amount').html($('#RewardAmount').val() || "0");
  } else {
    setrReward.style.setProperty("display", "none", "important");
    rewardAmount.style.setProperty("display", "none", "important");
    $('#reward_price_amount').html("0");
  }

  // check if exclusive is checked or note
  let isCheckedExclusive = $('#IsExclusiveAd').is(":checked") ? true : false;

  if (isCheckedExclusive) {
    exclusivePeriod.style.setProperty("display", "flex", "important");
  } else {
    exclusivePeriod.style.setProperty("display", "none", "important");
  }
});

// Define the UI Elements
const rewardCheck = document.querySelector("#IsReward");
const exclusiveCheck = document.querySelector("#IsExclusiveAd");
const exclusivePeriod = document.querySelector(".exclusiveperiod");
const setrReward = document.querySelector(".setreward");
const rewardAmount = document.querySelector(".rewardamount");
const timePeriod = document.querySelector("#AdDuration");
const groupAddOn = document.querySelector("#AdStartDate");
const adend = document.querySelector(".adend");
const price = document.querySelector("#PriceCategoryId");
const pricefixed = document.querySelector(".pricefixed");
const pricerange = document.querySelector(".pricerange");
const exclusiveslect = document.querySelector("#ExclusiveAdPeriod");
const rewardAmountField = document.querySelector("#RewardAmount");

///////////////////////// price Start ////////////////////////////

price.addEventListener("change", () => {
  if (price.value === "0") {
    pricefixed.style.setProperty("display", "none", "important");
    pricerange.style.setProperty("display", "none", "important");
    $('#PriceMin').val('');
    $('#PriceMax').val('');
    $('#Price').val('');
  } else if (price.value === "1") {
    pricefixed.style.setProperty("display", "none", "important");
    pricerange.style.setProperty("display", "none", "important");
    $('#PriceMin').val('');
    $('#PriceMax').val('');
    $('#Price').val('');
  } else if (price.value === "2") {
    pricefixed.style.setProperty("display", "none", "important");
    pricerange.style.setProperty("display", "flex", "important");
    $('#PriceMin').val('');
    $('#PriceMax').val('');
    $('#Price').val('');
  } else if (price.value === "3") {
    pricefixed.style.setProperty("display", "flex", "important");
    pricerange.style.setProperty("display", "none", "important");
    $('#PriceMin').val('');
    $('#PriceMax').val('');
    $('#Price').val('');
  }
});

///////////////////////// price End ////////////////////////////

///////////////////////// calender-start //////////////////////

const newTime = (date) => {
  let currentDay = date.getDate();
  let currentYear = date.getFullYear();
  let currentMOnth = date.getMonth() + 1;
  let check = "";

  if (currentMOnth < 10) {
    currentMOnth = "0" + currentMOnth;
  }

  if (currentDay < 10) {
    currentDay = "0" + currentDay;
  }

  return `${currentMOnth}/${currentDay}/${currentYear}`;
};


$("#AdStartDate").datepicker({ defaultDate: new Date() }); //at initialization
$("#AdStartDate").datepicker("setDate", new Date());
$("#AdStartDate").datepicker().on("changeDate", (e) => {
  adend.textContent = addDays(groupAddOn.value, parseInt(timePeriod.value));
  $('#ad_end_date_server_value').val(addDays(groupAddOn.value, parseInt(timePeriod.value)));
});

adend.textContent = groupAddOn.value;



timePeriod.addEventListener("change", (e) => {

  adend.textContent = addDays($("#AdStartDate").val(), parseInt(e.target.value));
  $('#ad_end_date_server_value').val(addDays($("#AdStartDate").val(), parseInt(e.target.value)));

  // refresh exculsive ad
  $('#period_ad_exclusive').html('');
  $('#period_ad_price').html('0.00');

  // CAlculateTOtalAdPRice
  $('#total_ad_price').html(CalculateTotalAdPrice());

  //if (e.target.value === "30") {
  //  exclusiveslect.innerHTML = `<option value="0">Select period</option><option value="2">1 day</option>
  //      <option value="2">2 days</option>
  //      <option value="3">1 week</option>
  //      <option value="4">2 weeks</option>
  //      <option value="5">1 month</option>`;
  //} else if (e.target.value === "60") {
  //  exclusiveslect.innerHTML = `<option value="0">Select period</option><option value="1">1 day</option>
  //      <option value="2">2 days</option>
  //      <option value="3">1 week</option>
  //      <option value="4">2 weeks</option>
  //      <option value="5">1 month</option>
  //      <option value="6">2 months</option>`;
  //} else if (e.target.value === "90") {
  //  exclusiveslect.innerHTML = `<option value="0">Select period</option><option value="1">1 day</option>
  //      <option value="2">2 days</option>
  //      <option value="3">1 week</option>
  //      <option value="4">2 weeks</option>
  //      <option value="5">1 months</option>
  //      <option value="6">2 months</option>
  //      <option value="7">3 months</option>`;
  //} else if (e.target.value === "180") {
  //  exclusiveslect.innerHTML = `<option value="0">Select period</option><option value="1">1 day</option>
  //      <option value="2">2 days</option>
  //      <option value="3">1 week</option>
  //      <option value="4">2 weeks</option>
  //      <option value="5">1 month</option>
  //      <option value="6">2 months</option>
  //      <option value="7">3 months</option>
  //      <option value="8">6 months</option>`;
  //} else if (e.target.value === "365") {
  //  exclusiveslect.innerHTML = `<option value="0">Select period</option><option value="1">1 day</option>
  //      <option value="2">2 days</option>
  //      <option value="3">1 week</option>
  //      <option value="4">2 weeks</option>
  //      <option value="5">1 month</option>
  //      <option value="6">2 months</option>
  //      <option value="7">3 months</option>
  //      <option value="8">6 months</option>
  //      <option value="9">1 year</option>`;
  //}

});

function addDays(date, days) {
  var result = new Date(date);
  result.setDate(result.getDate() + days);
  return newTime(result);
}


////////////////// calender-end/////////////////////

////////////////// rewardCheck-start /////////////////////

rewardCheck.addEventListener("click", () => {
  let isChecked = $('#IsReward').is(":checked") ? true : false;
  if (isChecked) {
    setrReward.style.setProperty("display", "flex", "important");
    rewardAmount.style.setProperty("display", "flex", "important");
    $('#reward_price_amount').html($('#RewardAmount').val() || "0");
  } else {
    setrReward.style.setProperty("display", "none", "important");
    rewardAmount.style.setProperty("display", "none", "important");
    $('#reward_price_amount').html("0");
  }
});


////////////////// rewardCheck-end /////////////////////

////////////////// exclusiveCheck-End /////////////////////

exclusiveCheck.addEventListener("click", () => {
  let isChecked = $('#IsExclusiveAd').is(":checked") ? true : false;

  if (isChecked) {
    exclusivePeriod.style.setProperty("display", "flex", "important");
  } else {
    exclusivePeriod.style.setProperty("display", "none", "important");
  }
});


////////////////// exclusiveCheck-End /////////////////////

rewardAmountField.addEventListener('keyup', (e) => {
  $('#reward_price_amount').html(e.target.value);
});

exclusiveslect.addEventListener("change", (e) => {
  $('#period_ad_price').html(e.target.value);
  $('#period_ad_exclusive').html($("#ExclusiveAdPeriod option:selected").html());

  // CAlculateTOtalAdPRice
  $('#total_ad_price').html(CalculateTotalAdPrice());
});

function CalculateTotalAdPrice() {
  // get the ad price ad exclusive ad orice
  let adPrice = parseFloat($('#ad_price').html());
  let exclusiveAdPrice = parseFloat($('#period_ad_price').html());

  let totalAdPrice = adPrice + exclusiveAdPrice

  return totalAdPrice;
}


