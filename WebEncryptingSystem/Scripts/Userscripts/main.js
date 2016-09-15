﻿

angular.module('EncryptApp', ['angularFileUpload'])
  .controller('AppController', function ($scope, $http, FileUploader) {

      var uploader = $scope.uploader = new FileUploader({
          url:"/home/fileupload"
      });

      uploader.onSuccessItem = function(item, response, status, headers) { 
          console.info('response', response);

          var resp = JSON.parse(response);
          $scope.filetxt = resp.File;
          $scope.fileName = resp.Name;
          uploader.clearQueue();
      }

      uploader.onErrorItem = function (item, response, status, headers) {
          alert("Dowloading failed! Unreadable file!");
          $scope.clear();
      }

      $scope.upload = function () {
          uploader.queue[0].upload();
      }

      $scope.clear = function() {
          uploader.clearQueue();
          $scope.filetxt = "";
      }

      $scope.spectrum = function () {
          $http.post("/home/spectrum", { "filename": $scope.fileName})
          .then(function (response) {
              $scope.spectrumStore = JSON.parse(response.data);
              $scope.chars = $scope.spectrumStore.CharsCount;
              $scope.letters = $scope.spectrumStore.LettersCount;

              console.log($scope.spectrumStore.Alphabet);

              var ctx = $("#chart");
              var myBarChart = new Chart(ctx,
              {
                  type: 'bar',
                  data: {
                      labels: $scope.spectrumStore.Alphabet,
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
  });




