angular.module("contosoApp").controller("StudentController", function ($scope, $http, $resource, $stateParams, toastr) {

    var studentService = $resource("http://localhost:56558/api/student", {}, {
        get: {method: "GET"},
        save: {method: "POST"},
        getById: { method: "GET", params: { id: '@id' } },
        remove: {method: "DELETE"},
        update: { method: "PUT" },
    });

    var formData = new FormData();

    $scope.getStudentPhoto = function ($files) {
        angular.forEach($files, function (value, key) {
            formData.append(key, value);
        });
    }

    var uploadStudentPhoto = function (studentId) {
        var studentPhotoObj = {
            method: "post",
            url: "http://localhost:56558/api/upload-student-photo/" + studentId,
            data: formData,
            headers: {
                'Content-type': undefined
            }
        }
        $http(studentPhotoObj);
    }

    $scope.saveStudent = function (data) {
        // MANUAL APPROACH
        $http.post("http://localhost:56558/api/student", data).then(function (response) {
            if (response.status === 201 && response.statusText == "Created") {
                uploadStudentPhoto(response.data.Id);
                toastr.success("Student information saved successfully");
            }
        }, function (error) {
            toastr.error("An unexpected error occured!");
        });

        // USING RESOURCE SERVICE (FROM ANGULARJS)
        //studentService.save(data);
    }

    $scope.getStudents = function () {
        // MANUAL APPROACH
        //$http.get("http://localhost:56558/api/student").then(function (response) {
        //    $scope.students = response.data;
        //});

        // USING RESOURCE SERVICE (FROM ANGULARJS)
        $scope.students = studentService.query();
    }

    $scope.getStudentById = function () {
        // MANUAL APPROACH
        //$http.get("http://localhost:56558/api/student/" + $stateParams.studentId)
        //    .then(function (response) {
        //        $scope.student = response.data;
        //});

        // USING CUSTOM MADE RESOURCE SERVICE
        $scope.student = studentService.getById({ id: $stateParams.studentId });
    }

    $scope.updateStudent = function (student) {
        // MANUAL APPROACH
        //$http.put("http://localhost:56558/api/student/" + $stateParams.studentId, student)
        //    .then(function (response) { });

        // USING RESOURCE SERVICE
        studentService.update(student);
    }

    $scope.deleteStudent = function (studentId) {
        $http.delete("http://localhost:56558/api/student/" + studentId)
            .then(function (response) { });
    }
});