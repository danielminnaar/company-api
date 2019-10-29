$(document).ready(function() {
    var token = "";

        $("#btnAuth").click(function(){
            $.post( "http://localhost:8000/company/authenticate", { Username: "companyadmin", Password: "password" })
                .done(function( data ) {
                    alert( "Data Loaded: " + data );
                });
        });   
        
        $("#btnGetAll").click(function(){
            alert("get all");
        });    

        $("#btnGetCompany").click(function(){
            alert("get company");
        });  
        
        $("#btnCreateCompany").click(function(){
            alert("create company");
        });  
});
