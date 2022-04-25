<?php

// Define the database to connect to
define("DBHOST", "localhost");
define("DBNAME", "id16746895_leaderboardbd");
define("DBUSER", "id16746895_leaderboardusn");
define("DBPASS", "XQ1+L*oq#[qr7LQ?");


// Connect to the database
$connection = new mysqli(DBHOST, DBUSER, DBPASS, DBNAME);

// Other code goes here
// Check if the request is to retrieve or post a score to the leaderboard
if (isset($_POST['retrieve_leaderboard']))
{
    // Create the query string
    $sql = "SELECT * FROM IdleBotsLB ORDER BY score DESC limit 50";

    // Execute the query
    $result = $connection->query($sql);
    $num_results = $result->num_rows;

    // Loop through the results and print them out, using "\n" as a delimiter
    for ($i = 0; $i < $num_results; $i++) 
    {
        if (!($row = $result->fetch_assoc()))
            break;
        echo $row["name"];
        echo "\n";
        echo $row["score"];
        echo "\n";
    }

    $result->free_result();
} elseif (isset($_POST['post_leaderboard']))
{
    // Get the user's name and store it
    $name = mysqli_escape_string($connection, $_POST['name']);
    $name = filter_var($name, FILTER_SANITIZE_STRING, FILTER_FLAG_STRIP_LOW | FILTER_FLAG_STRIP_HIGH);
    // Get the user's score and store it
    $score = $_POST['score'];

    // Create prepared statement
    $statement = $connection->prepare("INSERT INTO IdleBotsLB (name, score) VALUES (?, ?)");
    $statement->bind_param("si", $name, $score);

    $statement->execute();
    $statement->close();
}

// Close the database connection
$connection->close();

?>