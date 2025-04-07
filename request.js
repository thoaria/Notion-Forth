const dotenv = require('dotenv');
dotenv.config()
const { Client: NotionClient } = require("@notionhq/client");
const notion = new NotionClient({ auth: process.env.NOTION_KEY });

// have the gforth program take command line arguments that will then be input
// into the json for this request

process.argv.forEach(function (val, index, array) {
    console.log(index + ': ' + val);
  });