<h1 align="center">JellyPresence</h1>
Show what you are watching on Jellyfin as Rich Presence on Discord.

As long as Jellyfin Media Player is running, JellyPresence will remain active and update your Discord status with whatever you are viewing. JellyPresence will terminate itself after Jellyfin Media Player closes, so you will never have to worry about it running while you are not viewing anything.

# Requirements
<a href="https://dotnet.microsoft.com/en-us/download/dotnet/9.0">.NET 9.x</a> <br />
<a href="https://jellyfin.org/downloads/server">Jellyfin Server</a><br />
<a href="https://jellyfin.org/downloads">Jellyfin Media Player</a><br />
<a href="https://discord.com/">Discord Client</a><br />
<a href="https://discord.com/developers/applications">A Discord Application</a><br />
### Optional
<a href="https://www.python.org/downloads/">Python 3.x </a>

# Setup
<ol>
	<li> <h4> Create a Discord Application </h4></li>
		<ol>
			<li> <a href="https://discord.com/developers/applications"> Go to the Discord Developer Portal </a> </li>
			<li> Create a new application, the name you give it will be what shows up on Discord as "Watching" </li>
			<li> Copy the <strong> Application ID</strong> </li>
		</ol>
	<li> <h4> Create an API Key on Jellyfin Server </h4></li>
		<ol>
			<li> Log into an Admin account </li>
			<li> Go to the server <strong> Dashboard</strong></li>
			<li> Navigate to <strong> Advanced > API Keys </strong> </li>
			<li> Create a new key (the name does not matter) </li>
			<li> Copy the created <strong> API Key </strong> </li>
		</ol>
	<li> <h4> Create a ".env" file </h4></li>
		<ol>
			<li> <h4>Using Python</h4> </li>
				<ol>
					<li> In a terminal, run <strong>generate_env.py</strong> and follow the on screen instructions</li>
					<code>python3 generate_env.py</code>
				</ol>
			<li> <h4>Manually create the file</h4></li>
				<ol>
					<li> Create a file titled called ".env" in the same folder as JellyPresence.exe </li>
					<li> Open the file with Notepad or your text editor of choice and enter the following information</li>
<code>CLIENTID="DISCORD APPLICATION ID"</code><br />
<code>JELLYAPIKEY="JELLYFIN API KEY"</code><br />
<code>JELLYURL="JELLYFIN URL"</code><br />
<code>JELLYPATH="FILE PATH TO JELLYFINMEDIAPLAYER.EXE"</code>
					<li> Replace every quoted value with your own values</li>
				</ol>
		</ol>
	<li> <h4> OPTIONAL: Create a shortcut to JellyPresence </h4></li>
	<li> <h4> Launch JellyPresence instead of JMP whenever you want to use JMP</h4></li>
</ol>
 
