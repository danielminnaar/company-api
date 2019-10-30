$(document).ready(function() {
        var token = "";
        var host = $("#txtHost").val();
        var createToggle = false;
        $("#companyDetails").hide();
        $("#btnUpdateHost").click(function() {
            host = $("#txtHost").val();
        });
        
        // general process:
        // 1. Handle event
        // 2. Populate/check data
        // 3. Prepare HTTP request
        // 4. Handle response
        $("#btnAuth").click(function(){
            var credentials = { "Username": "companyadmin",
                                "Password": "password"}
                                
            $.ajax({
                url: host + "/authenticate",
                type: "POST",
                crossDomain: true,
                data: JSON.stringify(credentials),
                headers: { 
                    'Accept': 'text/plain',
                    'Content-Type': 'application/json' 
                },
                success: function (response) {
                    token = response;
                    updateConsole(new Date() + " => " + "JWT authentication successful; token updated.");
                },
                error: function (xhr, status) {
                    updateConsole("Authenticate error: " + xhr.status + ", " + xhr.statusText+ ", " + xhr.responseText);
                }
            });
        });   
        
        $("#btnGetAll").click(function(){
            $.ajax({
                url: host + "/",
                type: "GET",
                crossDomain: true,
                headers: { 
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + token
                },
                success: function (response) {
                    var companiesBody = "";
                    response.forEach(element => {
                        companiesBody += "<label>Id: " + element.id.toString() + ", Name: " + element.name + ", Exchange: " + element.exchange + ", Ticker: " + element.ticker + ", ISIN: " + element.isin + ", Website: " + element.website + "</label><br/>";
                    });
                    updateConsole(companiesBody);
                },
                error: function (xhr, status) {
                    console.log(xhr);
                    updateConsole("Get all companies error: " + xhr.status + ", " + xhr.statusText+ ", " + xhr.responseText);
                }
            });
        });    

        $("#btnGetCompany").click(function(){
            $.ajax({
                url: host + "/1",
                type: "GET",
                crossDomain: true,
                headers: { 
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + token
                },
                success: function (response) {
                    var companyBody = "";
                    companyBody += "<label>Id: " + response.id.toString() + ", Name: " + response.name + ", Exchange: " + response.exchange + ", Ticker: " + response.ticker + ", ISIN: " + response.isin + ", Website: " + response.website + "</label><br/>";
                    updateConsole(companyBody);
                },
                error: function (xhr, status) {
                    console.log(xhr);
                    updateConsole("Get company (1) error: " + xhr.status + ", " + xhr.statusText+ ", " + xhr.responseText);
                }
            });
        });  
        
        $("#btnCreate").click(function(){
            var isinCheck = new RegExp('^[A-Z]{2}[A-Z0-9]{10}');
            var isin = $("#txtISIN").val();
            if (!isinCheck.test(isin)) {
                updateConsole("Invalid ISIN!");
                return;
            }
            var company = {
                Name: $("#txtName").val(),
                Exchange: $("#txtExchange").val(),
                Ticker: $("#txtTicker").val(),
                ISIN: isin,
                Website: $("#txtWebsite").val()
            }
            $.ajax({
                url: host + "/",
                type: "POST",
                crossDomain: true,
                headers: { 
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + token
                },
                data: JSON.stringify(company),
                success: function (response) {
                    updateConsole(response);
                },
                error: function (xhr, status) {
                    console.log(xhr);
                    updateConsole("Create company error: " + xhr.status + ", " + xhr.statusText + ", " + xhr.responseText);
                }
            });
            
        });  

        $("#btnCreateCompany").click(function() {
            if(createToggle == false) {
                $("#companyDetails").show();
                createToggle = true;
            }
            else {
                $("#companyDetails").hide();
                createToggle = false;
            }
            
        });
        
        function updateConsole(msg) {
            $("#lblOutput").html(msg);
        }
});
