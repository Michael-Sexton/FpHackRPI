<html>

<head>

 <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    
    <title>CrashNY</title>

    <style>
     body{
        font-weight: bold;
        font-size: 20px;
    }
.form {
  height:auto;
  font-weight: bold;
  line-height: 2.5;
 	border-radius: 20px;
  	padding-left: 20px;
    padding-right: 20px;
    padding-bottom: 20px;
    text-align: center;
  margin-left: 50px;
	width:400px; 
  	margin-right:auto; 
  	margin-left:auto; 
  	border:5px solid black;
    
    }
    
  </style>
    
</head>

<body>
    <h1>
        <center>Crash NY</center>
    </h1>
    
    <form name="form" action="result.php" method="post";>
        <p>Select a Borough:</p>
        <select class="form-control col-xs-4" name="borough">
          <option selected value="0" disabled="disabled">Select Borough</option>
          <option value="1">Manhattan</option>
          <option value="2">Brooklyn</option>
          <option value="3">Queens</option>
          <option value="4">The Bronx</option>
          <option value="5">Staten Island</option>
      </select>

        <p>Select a Time of Day:</p>
        <select class="form-control col-xs-4" name="time">
          <option selected disabled="disabled">Select Time</option>
          <option value="0">12am</option>
          <option value="1">1am</option>
          <option value="2">2am</option>
          <option value="3">3am</option>
          <option value="4">4am</option>
          <option value="5">5am</option>
          <option value="6">6am</option>
          <option value="7">7am</option>
          <option value="8">8am</option>
          <option value="9">9am</option>
          <option value="10">10am</option>
          <option value="11">11am</option>
          <option value="12">12pm</option>
          <option value="13">1pm</option>
          <option value="14">2pm</option>
          <option value="15">3pm</option>
          <option value="16">4pm</option>
          <option value="17">5pm</option>
          <option value="18">6pm</option>
          <option value="19">7pm</option>
          <option value="20">8pm</option>
          <option value="21">9pm</option>
          <option value="22">10pm</option>
          <option value="23">11pm</option>
          <option value="24">12pm</option>
      </select>
      <br>

    </form>
 <input class="btn btn-primary" type="submit" value="Enter">

</body>

</html>
