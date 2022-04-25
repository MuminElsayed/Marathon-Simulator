<?php

// Define the database to connect to
define("DBHOST", "localhost");
define("DBNAME", "id16746895_leaderboardbd");
define("DBUSER", "id16746895_leaderboardusn");
define("DBPASS", "XQ1+L*oq#[qr7LQ?");


// Connect to the database
$connection = new mysqli(DBHOST, DBUSER, DBPASS, DBNAME);

// Other code goes here
// Check if the request is to retrieve or post a playerTime to the leaderboard
if (isset($_POST['retrieve_leaderboard']))
{
    // Create the query string
    $sql = "SELECT * FROM MarathonRunnerLB ORDER BY PlayerTime DESC limit 50";

    // Execute the query
    $result = $connection->query($sql);
    $num_results = $result->num_rows;

    // Loop through the results and print them out, using "\n" as a delimiter
    for ($i = 0; $i < $num_results; $i++) 
    {
        if (!($row = $result->fetch_assoc()))
            break;
        echo $row["PlayerName"];
        echo "\n";
        echo $row["AvgSpeed"];
        echo "\n";
        echo $row["PlayerTime"];
        echo "\n";
        echo $row["MapNum"];
        echo "\n";
    }

    $result->free_result();
} elseif (isset($_POST['post_leaderboard']))
{
    // Get the user's name and store it
    $PlayerName = mysqli_escape_string($connection, $_POST['PlayerName']);
    $PlayerName = filter_var($PlayerName, FILTER_SANITIZE_STRING, FILTER_FLAG_STRIP_LOW | FILTER_FLAG_STRIP_HIGH);
    // Get the user's avgSpeed and store it
    $AvgSpeed = $_POST['AvgSpeed'];
    // Get the user's PlayerTime and store it
    $PlayerTime = $_POST['PlayerTime'];
    // Get the user's MapNum and store it
    $MapNum = $_POST['MapNum'];
    // Create prepared statement
    $statement = $connection->prepare("INSERT INTO MarathonRunnerLB (PlayerName, AvgSpeed, PlayerTime, MapNum) VALUES (?, ?, ?, ?)");
    $statement->bind_param("siii", $PlayerName, $AvgSpeed, $PlayerTime, $MapNum);

    $statement->execute();
    $statement->close();
}

// Close the database connection
$connection->close();

?>