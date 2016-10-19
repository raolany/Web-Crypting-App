$(".parametersAffine, .parametersBit, .parametersHill, .parametersVigenere").hide();
var m = 41;
var MAXNUM = 999999;

angular.module('EncryptApp', ['angularFileUpload'])
  .controller('AppController', function ($scope, $http, FileUploader) {

      var uploader = $scope.uploader = new FileUploader({
          url: "/home/fileUploadOnServer"
      });

      uploader.onSuccessItem = function(item, response, status, headers) { 
          console.info('response', response);

          var resp = JSON.parse(response);
          $scope.filetxt = resp.File;
          $scope.fileName = resp.Name;
          //uploader.clearQueue();
          //$("#id_fileloader").prop('value', null);
      }

      uploader.onErrorItem = function (item, response, status, headers) {
          alert("Dowloading failed! Unreadable file!");
          $scope.clear();
      }

      $scope.upload = function () {
          if (uploader.queue.length > 0)
              uploader.queue[uploader.queue.length-1].upload();
          else
              alert("Choose a file");
      }

      $scope.clear = function() {
          uploader.clearQueue();
          $("#id_fileloader").prop('value', null);
          $scope.filetxt = "";
          $scope.affineA = "";
          $scope.affineB = "";
          $scope.outputfilename = "";
          $scope.outputtxt = "";
          $scope.fileName = "";
      }

      $scope.spectrum = function () {
          $http.post("/home/spectrum", { "filename": $scope.fileName})
          .then(function (response) {
              $scope.spectrumStore = JSON.parse(response.data);
              $scope.chars = $scope.spectrumStore.CharsCount;
              $scope.letters = $scope.spectrumStore.LettersCount;

              $scope.spectrum = $scope.spectrumStore.Spectrum;

              console.log($scope.spectrumStore.AlphabetTmp);

              var ctx = $("#chart");
              var myBarChart = new Chart(ctx,
              {
                  type: 'bar',
                  data: {
                      labels: $scope.spectrumStore.AlphabetTmp,
                      datasets: [
                          {
                              label: "Spectrum Analises",
                              borderWidth: 1,
                              data: $scope.spectrumStore.Spectrum
                          }
                      ]
                  },
                  options: {
                      scales: {
                          xAxes: [
                              {
                                  stacked: true
                              }
                          ],
                          yAxes: [
                              {
                                  stacked: true
                              }
                          ]
                      }
                  }
              });

          }, function errorCallback(response) {
              console.log("error");
          });
      }

      $scope.changeMethod = function () {
          if ($scope.selectMethod == 1) {
              $(".parametersAffine").show();
              $(".parametersBit, .parametersHill, .parametersVigenere").hide();
          }
          else if ($scope.selectMethod == 2) {
              $(".parametersBit").show();
              $(".parametersAffine, .parametersHill, .parametersVigenere").hide();
          }
          else if ($scope.selectMethod == 3) {
              $(".parametersVigenere").show();
              $(".parametersAffine, .parametersBit, .parametersHill").hide();
          }
          else if ($scope.selectMethod == 4) {
              $(".parametersHill").show();
              $(".parametersAffine, .parametersBit, .parametersVigenere").hide();
          }
      }

      $scope.encrypt = function () {

          if ($scope.selectMethod == 1) {

              if ($("#affine_a").hasClass("alert-danger") || $("#affine_b").hasClass("alert-danger")) {
                  alert("Correct errors");
              }
              else if ($scope.fileName === undefined || $scope.fileName === "") {
                  alert("Choose a file");
              }
              else if ($scope.affineA === undefined || $scope.affineB === undefined || $scope.affineA === "" || $scope.affineB === "") {
                  alert("Enter the keys");
              }
              else {
                  $http.post("/home/AffineFileCrypting", { "filename": $scope.fileName, "enc": true, "a": $scope.affineA, "b": $scope.affineB })
                  .then(function (response) {

                      console.log("AffineFileCrypting", response.data);

                      var filemodel = JSON.parse(response.data);
                      $scope.outputtxt = filemodel.File;
                      $scope.outputfilename = filemodel.Name;

                  }, function errorCallback(response) {
                      console.log("error", "/home/AffineFileCrypting:: filename: "+ $scope.fileName+",enc: "+ true+ ",a: "+ $scope.affineA+ ",b: "+$scope.affineB);
                  });
              }

          }
          else if ($scope.selectMethod == 2) {

              if ($("#bit1").hasClass("alert-danger") || $("#bit2").hasClass("alert-danger")) {
                  alert("Correct errors");
              }
              else if ($scope.fileName === undefined || $scope.fileName === "") {
                  alert("Choose a file");
              }
              else if ($scope.bit1 === undefined || $scope.bit2 === undefined || $scope.bit1 === "" || $scope.bit2 === "") {
                  alert("Enter the keys");
              }
              else {
                  $http.post("/home/BitFileCrypting", { "filename": $scope.fileName, "a": $scope.bit1, "b": $scope.bit2, "act": true })
                  .then(function (response) {

                      console.log("BitFileCrypting", response.data);

                      var filemodel = JSON.parse(response.data);
                      $scope.outputtxt = filemodel.File;
                      $scope.outputfilename = filemodel.Name;

                  }, function errorCallback(response) {
                      console.log("error", "/home/BitFileCrypting:: filename: " + $scope.fileName + ",enc: " + true + ",a: " + $scope.bit1 + ",b: " + $scope.bit2);
                  });
              }

          }
          else if ($scope.selectMethod == 3) {

              if ($("#vigenereKey").hasClass("alert-danger")) {
                  alert("Correct errors");
              }
              else if ($scope.fileName === undefined || $scope.fileName === "") {
                  alert("Choose a file");
              }
              else if ($scope.vigenereKey === undefined || $scope.vigenereKey === "") {
                  alert("Enter the key");
              }
              else {
                  $http.post("/home/VigenereFileCrypting", { "filename": $scope.fileName, "act": true, "key": $scope.vigenereKey })
                  .then(function (response) {

                      console.log("VigenereFileCrypting", response.data);

                      var filemodel = JSON.parse(response.data);
                      $scope.outputtxt = filemodel.File;
                      $scope.outputfilename = filemodel.Name;

                  }, function errorCallback(response) {
                      console.log("error", "/home/VigenereFileCrypting:: filename: " + $scope.fileName + ",enc: " + true + ",key: " + $scope.vigenereKey);
                  });
              }

          }
          else if ($scope.selectMethod == 4) {

              if ($("#hill01").hasClass("alert-danger") || $("#hill02").hasClass("alert-danger") || $("#hill03").hasClass("alert-danger") || $("#hill04").hasClass("alert-danger")) {
                  alert("Correct errors");
              }
              else if ($scope.fileName === undefined || $scope.fileName === "") {
                  alert("Choose a file");
              }
              else if ($scope.hill01 === undefined || $scope.hill02 === undefined || $scope.hill03 === undefined || $scope.hill04 === undefined ||
                        $scope.hill01 === "" || $scope.hill02 === "" || $scope.hill03 === "" || $scope.hill04 === "") {
                  alert("Enter the keys");
              }
              else {
                  var key = [$scope.hill01, $scope.hill02, $scope.hill03, $scope.hill04];
                  console.log(key);
                  
                  $http.post("/home/HillFileCrypting", { "filename": $scope.fileName, "act": true , "key": key })
                  .then(function (response) {

                      console.log("HillFileCrypting", response.data);

                      var filemodel = JSON.parse(response.data);
                      $scope.outputtxt = filemodel.File;
                      $scope.outputfilename = filemodel.Name;

                  }, function errorCallback(response) {
                      console.log("error", "/home/HillFileCrypting:: filename: " + $scope.fileName + ",act: " + true + ",key: "+ key);
                  });
              }

          }
          else {
              alert("Choose an encrytion method");
          }
      }

      $scope.decrypt = function () {

          if ($scope.selectMethod == 1) {

              if ($("#affine_a").hasClass("alert-danger") || $("#affine_b").hasClass("alert-danger")) {
                  alert("Correct errors");
              }
              else if ($scope.fileName === undefined || $scope.fileName === "") {
                  alert("Choose a file");
              }
              else if ($scope.affineA === undefined || $scope.affineB === undefined || $scope.affineA === "" || $scope.affineB === "") {
                  alert("Enter the keys");
              }
              else {
                $http.post("/home/AffineFileCrypting", { "filename": $scope.fileName, "enc": false, "a": $scope.affineA, "b": $scope.affineB })
                  .then(function (response) {

                      console.log("AffineFileCrypting", response.data);

                      var filemodel = JSON.parse(response.data);
                      $scope.outputtxt = filemodel.File;
                      $scope.outputfilename = filemodel.Name;

                  }, function errorCallback(response) {
                        alert("{Error}-" + response.status+" "+response.statusText);
                      console.log(response);
                      console.log("error", "/home/AffineFileCrypting:: filename: " + $scope.fileName + ",enc: " + false + ",a: " + $scope.affineA + ",b: " + $scope.affineB);
                  });
             }

          }
          else if ($scope.selectMethod == 2) {

              if ($("#bit1").hasClass("alert-danger") || $("#bit2").hasClass("alert-danger")) {
                  alert("Correct errors");
              }
              else if ($scope.fileName === undefined || $scope.fileName === "") {
                  alert("Choose a file");
              }
              else if ($scope.bit1 === undefined || $scope.bit2 === undefined || $scope.bit1 === "" || $scope.bit2 === "") {
                  alert("Enter the keys");
              }
              else {
                  $http.post("/home/BitFileCrypting", { "filename": $scope.fileName, "act": false, "a": $scope.bit1, "b": $scope.bit2 })
                  .then(function (response) {

                      console.log("BitFileCrypting", response.data);

                      var filemodel = JSON.parse(response.data);
                      $scope.outputtxt = filemodel.File;
                      $scope.outputfilename = filemodel.Name;

                  }, function errorCallback(response) {
                      console.log("error", "/home/BitFileCrypting:: filename: " + $scope.fileName + ",act: " + false + ",a: " + $scope.bit1 + ",b: " + $scope.bit2);
                  });
              }

          }
          else if ($scope.selectMethod == 3) {

              if ($("#vigenereKey").hasClass("alert-danger")) {
                  alert("Correct errors");
              }
              else if ($scope.fileName === undefined || $scope.fileName === "") {
                  alert("Choose a file");
              }
              else if ($scope.vigenereKey === undefined || $scope.vigenereKey === "") {
                  alert("Enter the key");
              }
              else {
                  $http.post("/home/VigenereFileCrypting", { "filename": $scope.fileName, "act": false, "key": $scope.vigenereKey })
                  .then(function (response) {

                      console.log("VigenereFileCrypting", response.data);

                      var filemodel = JSON.parse(response.data);
                      $scope.outputtxt = filemodel.File;
                      $scope.outputfilename = filemodel.Name;

                  }, function errorCallback(response) {
                      console.log("error", "/home/VigenereFileCrypting:: filename: " + $scope.fileName + ",enc: " + false + ",key: " + $scope.vigenereKey);
                  });
              }

          }
          else if ($scope.selectMethod == 4) {

              if ($("#hill01").hasClass("alert-danger") || $("#hill02").hasClass("alert-danger") || $("#hill03").hasClass("alert-danger") || $("#hill04").hasClass("alert-danger")) {
                  alert("Correct errors");
              }
              else if ($scope.fileName === undefined || $scope.fileName === "") {
                  alert("Choose a file");
              }
              else if ($scope.hill01 === undefined || $scope.hill02 === undefined || $scope.hill03 === undefined || $scope.hill04 === undefined ||
                        $scope.hill01 === "" || $scope.hill02 === "" || $scope.hill03 === "" || $scope.hill04 === "") {
                  alert("Enter the keys");
              }
              else {
                  var key = [$scope.hill01, $scope.hill02, $scope.hill03, $scope.hill04];

                  $http.post("/home/HillFileCrypting", { "filename": $scope.fileName, "act": false, "key": key })
                  .then(function (response) {

                      console.log("HillFileCrypting", response.data);

                      var filemodel = JSON.parse(response.data);
                      $scope.outputtxt = filemodel.File;
                      $scope.outputfilename = filemodel.Name;

                  }, function errorCallback(response) {
                      console.log("error", "/home/HillFileCrypting:: filename: " + $scope.fileName + ",act: " + false + ",key: " + key);
                  });
              }

          }
          else {
              alert("Choose an encrytion method");
          }
      }

      $scope.checkAffineA = function () {
          if ($scope.affineA === "") {
              $("#affine_a").removeClass("alert-danger");
              return;
          }

          var a = parseInt($scope.affineA);
          if (!isNaN(a)) {
              if (a > 999999) {
                  $("#affine_a").addClass("alert-danger");
                  return;
              }

              if (NOD_Evc(a, m) !== 1 || a === 1) {
                  $("#affine_a").addClass("alert-danger");
              } else {
                  $("#affine_a").removeClass("alert-danger");
              }
          } 
      }

      $scope.checkAffineB = function () {
          if ($scope.affineA === "") {
              $("#affine_b").removeClass("alert-danger");
              return;
          }

          var b = parseInt($scope.affineB);
         
            if (b > MAXNUM) {
                $("#affine_b").addClass("alert-danger");
            } else {
                $("#affine_b").removeClass("alert-danger");
            }
      }

      $scope.checBit = function () {
          if ($scope.bit1 === "" || $scope.bit1 === "undefined") {
              $("#bit1").removeClass("alert-danger");
          }
          if ($scope.bit2 === "" || $scope.bit2 === "undefined") {
              $("#bit2").removeClass("alert-danger");
          }

          var bit1 = parseInt($scope.bit1);
          var bit2 = parseInt($scope.bit2);

            if (bit1 > 7) {
                $("#bit1").addClass("alert-danger");
            } else {
                $("#bit1").removeClass("alert-danger");
            }

          if (bit2 > 7) {
              $("#bit2").addClass("alert-danger");
          } else {
              $("#bit2").removeClass("alert-danger");
          }
      }

  });

function proverkaInt(input) {
    input.value = input.value.replace(/[^\d,]/g, '');
};


function NOD_Evc(a, b) {
    var r = 1;
    var q = 0;
       
    while (r != 0)
    {
        if (a >= b)
        {
            q = a/b;
            r = a%b;
            a = b;
            b = r;
        }
        else
        {
            q = b/a;
            r = b%a;
            b = a;
            a = r;
        }
    }
    return Math.max(a,b);
}

