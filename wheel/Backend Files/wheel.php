<?php

$servername = "localhost";
$username = "root";
$password = "";
$dbname = "wheel";
$dbwheelchance = "chance";

//user variables
$userLogin = $_POST["userLogin"];
$userWheeldata = $_POST["userWheeldata"];

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}
echo "Connected successfully <br><br>";

$sql = "SELECT username FROM users WHERE username = '" . $userLogin . "'";

$result = $conn->query($sql);

if ($result->num_rows > 0) {
  // output data of each row
  //while($row = $result->fetch_assoc()) {
  //  echo "username: " . $row["username"]. " - wheeldata: " . $row["wheeldata"]. "<br>";
  $sql3 = "UPDATE users SET wheeldata = '" . $userWheeldata . "' WHERE username='" . $userLogin . "'";
  if ($conn->query($sql3) === TRUE) {
    echo "New record created successfully";
  } else {
    echo "Error: " . $sql3 . "<br>" . $conn->error;
  }
  }
else {
  //echo "0 results";
  $sql2 = "INSERT INTO users (username, password, wheeldata)
  VALUES ('" . $userLogin . "', '55', '" . $userWheeldata . "')";
  if ($conn->query($sql2) === TRUE) {
    echo "New record created successfully";
  } else {
    echo "Error: " . $sql2 . "<br>" . $conn->error;
  }
}
$conn->close();

?>