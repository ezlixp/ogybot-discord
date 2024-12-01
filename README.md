<h1 align="center"> ogybot </h1>

<h2 align="center"> About </h2>

This is a discord bot made for the Idiot Co Discord server. This project is mainly maintained by me, but feel free to
open pull requests to merge new features, bug fixes, improvements, etc.


<h2 align="center"> Technologies </h2>

The bot itself is written in C# using the Discord.NET Library, however it does have an API implementation written in
Node.js, feel free to check it out <a href="https://github.com/ezlixp/ico_server" target="_blank">here</a>.


<h2 align="center"> Commands </h2>

- /info - Displays info about the bot, such as this repository and it's authors.
- /chiefs - Pings guild chiefs.
- /raid - Pings either Light/Heavy Raid roles.
- /online - Shows current mod users online in-game.
- /tomelist (and tomelist-add, tomelist-remove) - Displays players in the queue for a guild tome. /tomelist-add adds a
  player to queue and /tomelist-remove removes one based on their username or index.
- /waitlist (and waitlist-add, waitlist-remove) - Displays players in the queue for a guild invite. /waitlist-add adds a
  player to queue and /waitlist-remove removes one based on their username or index.
- /aspectlist (and aspectlist-decrement) - Displays the number of aspects owed to each guild member based on their raids
  completed. Aspectlist-decrement decrements their aspects owed counter by 1.


<h2 align="center"> Contributions </h2>

Contributions are heavily appreciated, however, before you commit please make sure the code is readable and doesn't
break anything.

<h3> How to contribute </h3>

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/newFeature`)
3. Commit your Changes (`git commit -m 'feat: add newFeature'`)
4. Push to the Branch (`git push origin feature/newFeature`)
5. Open a Pull Request


That's it! Thanks for checking out my little project!