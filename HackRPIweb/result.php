<html>
<style>
 <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    
    body{
        font-weight: bold;
        font-size: 20px;
        background-image: url("img/wallpaper.png");
    }
    html{
        background-image: url("img/wallpaper.png");
    }
 
    
    .form {
        height: auto;
        font-weight: bold;
        line-height: 2.5;
        border-radius: 20px;
        padding-left: 20px;
        padding-right: 20px;
        padding-bottom: 20px;
        text-align: center;
        margin-left: 50px;
        width: 400px;
        margin-right: auto;
        margin-left: auto;
        border: 5px solid black;
        background-color: white;
    }

</style>

<head>

    <title>Crash NY</title>
</head>

<body>
    <h1>
        <center  style="color: white; font-weight: bold; text-shadow: 4px 4px black;">Crash NY</center>
    </h1>

    <div class="form">
        <?php

    $borough = $_POST['borough'];
      $time = $_POST['time'];
         
         echo "<div style='text-align:center; font-size:20px;' class='col-xs-7'>Selected Borough:</div>";
         echo "<div style='text-align:center; font-size:20px;' class='col-xs-7'>$borough</div>";
         echo "<div style='text-align:center; font-size:20px;' class='col-xs-7'>Selected Time:</div>";
         echo "<div style='text-align:center; font-size:20px;' class='col-xs-7'>$time</div>";
     
?>
            <button class="btn btn-primary" onclick="history.go(-1);">Back</button>
    </div>


</body>

</html>
