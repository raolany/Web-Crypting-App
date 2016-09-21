$(".parametersAffine").hide();
$(".parametersBit").hide();

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
              uploader.queue[0].upload();
          else
              alert("Choose a file");
      }

      $scope.clear = function() {
          uploader.clearQueue();
          $("#id_fileloader").prop('value', null);
          $scope.filetxt = "";
      }

      $scope.spectrum = function () {
          $http.post("/home/spectrum", { "filename": $scope.fileName})
          .then(function (response) {
              $scope.spectrumStore = JSON.parse(response.data);
              $scope.chars = $scope.spectrumStore.CharsCount;
              $scope.letters = $scope.spectrumStore.LettersCount;

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
              $(".parametersBit").hide();
          }
          else if ($scope.selectMethod == 2) {
              $(".parametersBit").show();
              $(".parametersAffine").hide();
          }
      }

      $scope.encrypt = function () {

          if ($scope.selectMethod == 1) {

              $http.post("/home/AffineFileCrypting", { "filename": $scope.fileName, "enc": true, "a": $scope.affineA, "b":$scope.affineB })
                  .then(function (response) {

                      console.log("AffineFileCrypting", response.data);

                      var filemodel = JSON.parse(response.data);
                      $scope.outputtxt = filemodel.File;
                      $scope.outputfilename = filemodel.Name;

                  }, function errorCallback(response) {
                      console.log("error", "/home/AffineFileCrypting:: filename: "+ $scope.fileName+",enc: "+ true+ ",a: "+ $scope.affineA+ ",b: "+$scope.affineB);
                  });

          } else {
              alert("Choose an encrytion method");
          }
      }

      $scope.decrypt = function () {

          if ($scope.selectMethod == 1) {

              $http.post("/home/AffineFileCrypting", { "filename": $scope.fileName, "enc": false, "a": $scope.affineA, "b": $scope.affineB })
                  .then(function (response) {

                      console.log("AffineFileCrypting", response.data);

                      var filemodel = JSON.parse(response.data);
                      $scope.outputtxt = filemodel.File;
                      $scope.outputfilename = filemodel.Name;

                  }, function errorCallback(response) {
                      console.log("error", "/home/AffineFileCrypting:: filename: " + $scope.fileName + ",enc: " + true + ",a: " + $scope.affineA + ",b: " + $scope.affineB);
                  });

          } else {
              alert("Choose an encrytion method");
          }
      }

      //$scope.savefile = function () {
      //
      //    //if (angular.isDefined($scope.outputfileName)) {
      //    alert($scope.outputfilename);
      //    $http.post("/home/FileUploadOnPc", { "filename": $scope.outputfilename })
      //            .then(function(response) {
      //                    console.log(response.data);
      //                },
      //                function errorCallback(response) {
      //                    console.log("error", "/home/AffineFileCrypting:: outputfilename: " + $scope.outputfileName);
      //                });
      //    //} else {
      //    //    alert("{savefile} File not found");
      //    //}
      //}

  });



//int gcd(int a, int b)
//{
//    while (b != 0)
//b = a % (a = b);
//return a;
//}
