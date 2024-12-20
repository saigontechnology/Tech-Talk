const express = require('express')
const path = require('path');
const bodyParser = require('body-parser');

const app = express()
const port = process.env.PORT || 3002

app.use(express.json());

app.get('', (req,res)=> {
  res.send('Express.js V5')
})

/** 
 * Dropped Support for Older Node.js Versions
 * Express 5.0 requires Node.js 18 or higher
 */

/**
 * Updated to path-to-regexp@8.x for security reasons (ReDoS mitigation).
 * Regular expressions in route string are not supported
 * 
 */
// app.get('/:type(manager|employee)/:id', (req, res) => {
//   res.send(req.params);
// });

app.get(['/manager/:id', '/employee/:id'], (req, res) => {
  res.send(req.params);
});


/**
 * Alternatively, you can write a custom regex and pass it into the function. 
 * This solution only applies to common text, not work with dynamic route parameters)
 */
const customRegex = /^\/product-code\/[a-zA-Z0-9]{1,5}$/;
app.get(customRegex, (req, res) => {
   res.sendStatus(200);
});

const customRegex2 = /^\/category\/:id\/product-code\/[a-zA-Z0-9]{1,5}$/;
app.get(customRegex2, (req, res) => {
  res.send(req.params);
});

// app.get('/category/:id'+customRegex, (req, res) => {
//   res.send(req.params);
// });

// app.get('/category/:id/product-code/[a-zA-Z0-9]{1,5}', (req, res) => {
//   res.send(req.params);
// });


/**
 * Parameter names now support valid JavaScript identifiers, 
 * it can begin with a letter or underscore (_) or dollar sign ($)
 * or quoted like :"this".
 */
app.get('/product-detail-id/:_id', (req, res) => {
  res.send(req.params)
})

app.get('/product-detail-name/:$name', (req, res) => {
  res.send(req.params)
})

app.get('/product-detail-tag/:"tag"', (req, res) => {
  res.send(req.params)
})


/**
 * Some characters have been reserved to avoid confusion during upgrade (()[]?+!) 
 */
// app.get('/order(-item)?', (req, res) => {
//   res.sendStatus(200);
// })

// app.get('/client-detail/[a-zA-Z]{1,5}', (req, res) => {
//   res.sendStatus(200);
// });



/**
 * The wildcard * must have a name, matching the behavior of parameters :, 
 * use /*name instead of /*
 * URL: /clothes-product/shirt/pants/hat/coat/socks
 */
app.get('/clothes-product/*name', (req, res) => {
  res.send(req.params);
})


/**
 * The optional character ? is no longer supported, :name? becomes {:name}.
 */
app.get('/product-list{/:length}', (req, res) => {
  res.send(req.params);
})


/**
 * Promise support: Rejected promises handled from middleware and handlers
 * When a promise is rejected or has an error thrown, 
 * it can be passed to error handling middleware in Express 5. 
 */
app.get('/user-request/reject', () => {
  return Promise.reject('rejected');
});

app.get('/user-request/throw', async () => {
  return await Promise.resolve().then(() => {
      throw new Error('error');
  });
});


/**
 * body-parser & req.body changes:
 * Remove deprecated bodyParser()
 * Able to customize the urlencoded body depth, with a default value of 32
 * Default “extended” property of urlencoded parser is false
 * req.body is no longer always initialized to {}
 */
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true, parameterLimit: 2, depth: 1 }));

app.post('/user-info', (req, res) => {
  res.send(`User data: ${JSON.stringify(req.body)}`);
});

app.post('/product-info', (req, res) => {
  console.log('req.body',req.body);
  if (!req.body) {
    res.status(400).send('No data sent');
  } else {
    res.send(`Product data: ${JSON.stringify(req.body)}`);
  }
});

/**
 *  Reintroduced app.router
 */
const router = app.router;

router.get('/about', (req, res) => {
  res.send('Hi!');
});


/**
 * req.host
 * Return of the Port to req.host in Express 5
 */
 // Result: localhost:3002
app.get('/dashboard', (req, res) => {
  console.log('Log req.host: ', req.host);
  res.status(200).send(req.host);
})


/**
 * req.query is only a getter (readonly)
 * Default "query parser" has been changed from “extended” to “simple”.
 * sample URL: /employee-info?person[name]=bobby&person[age]=25
 */
app.set("query parser", "extended");
app.get('/employee-info', (req, res) => {
    req.query = { error: 'Error message: Not Found!'}; 
    res.send(req.query);
})

/**
 * res.status
 * Stricter Error Handling for Invalid Status Codes
 * accepts only integers, and input must be greater than 99 and less than 1000
 */
app.get('/request/invalid', (req, res) => {
  res.status(1000).send('Invalid status');
  // res.status(99).send('Invalid status');
  // res.status('a').send('Invalid status');
  // res.status(500).send('Invalid status');
})


/*
 * ********************************************************************************************************************************
 * Other Updates - Removed methods and properties
 * ********************************************************************************************************************************
 */
app.get('/new-redirect', (req, res) => {
  // Redirecting back to the referrer or falling back to the homepage if no referrer is found
  const redirectUrl = req.get('Referrer') || '/';
  res.redirect(redirectUrl);
});

app.get('/new-location', (req, res) => {
  // Setting the location header to the referrer or the homepage if referrer is unavailable
  const locationUrl = req.get('Referrer') || '/';
  res.location(locationUrl).send('Location header set to the previous page or homepage');
});


/**
 * Remove Methods and Properties
 * app.del -> app.delete
 * req.param(name) -> req.params
 * req.json(status, object) -> res.status(status).json(object)
 * req.send(status, object) -> res.status(status).send(object) 
 * req.send(status) -> req.sendStatus(status)
 */
app.delete('/test-delete/:id', (req, res) => {
  console.log('Log id: ', req.params.id);
  res.status(500).send({ error: 'Internal Server Error'});
})


/**
 * app.param(fn) -> app.param('name, fn)
 */
checkSpecialOption = (option) => {
    return (req, res, next, val) => {
        if (val == option) {
            res
                .status(500)
                .json({ isDuplicated: true, error: 'Selected option is duplicated with SPECIAL_OPTION' });
        } else {
            next();
        }
    };
};

const SPECIAL_OPTION = 1337;
app.param('option', checkSpecialOption(SPECIAL_OPTION));

app.get('/test-app-param1/:option', (req, res) => {
    res.sendStatus(200);
});

app.get('/test-app-param2/:option', (req, res) => {
    res.sendStatus(200);
});


/**
 * Rename and Pluralized method names
 * res.sendfile() -> res.sendFile()
 * req.acceptsCharset() -> req.acceptsCharsets()
 * req.acceptsEncoding() -> req.acceptsEncodings()
 * req.acceptsLanguage() -> req.acceptsLanguages()
 */
app.get('/test-sendfile', (req, res) => {
  req.acceptsCharsets();
  req.acceptsEncodings();
  req.acceptsLanguages();
  res.sendFile(path.join(__dirname, '/public/test.html'));
});

app.get('/test-json', (req, res) => {
  // res.json(200,{ user: 'user1' }); // Don't works
  res.status(200).json({ user: 'user1' })
});


/**
 * pathIsAbsolute(string) -> path.isAbsolute(string)
 */
app.get('/test-path-absolute', (req, res) => {
  // const isAbsolute = pathIsAbsolute('C:/Users');
  // const isAbsolute = pathIsAbsolute('src/db');
  const isAbsolute = path.isAbsolute('src/db')

  console.log('pathIsAbsolute: ',isAbsolute);
  res.sendStatus(200);
})


app.listen(port, () => {
  console.log('Server is up on port ' + port)
})