const express = require('express')
const bodyParser = require('body-parser');
const path = require('path');
const pathIsAbsolute = require('path-is-absolute');
const { error } = require('console');

const app = express()
const port = process.env.PORT || 3001

app.use(express.json())

app.get('', (req,res)=> {
  res.send('Express.js V4')
})

/** 
 * Dropped Support for Older Node.js Versions
 * Express 5.0 requires Node.js 18 or higher
 */


/**
 * Use path-to-regexp@0.1.x
 * Regular expressions in route string are supported
 */
app.get('/:type(manager|employee)/:id', (req, res) => {
  res.send(req.params);
});


/**
 * Use directly Regular Expressions in route string 
 * (allow dynamic route parameters can combine with it)
 */
app.get('/product-code/[a-zA-Z0-9]{1,5}', (req, res) => {
  res.sendStatus(200);
});

app.get('/category/:id/product-code/[a-zA-Z0-9]{1,5}', (req, res) => {
  res.send(req.params);
});


/**
 * Parameter names must begin with a letter or underscore (_)
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
 * Can use (()[]?+!)
 */
app.get('/order(-item)?', (req, res) => {
  res.sendStatus(200);
})

app.get('/client-detail/[a-zA-Z]{1,5}', (req, res) => {
  res.sendStatus(200);
});

/**
 * Use * to match zero or more characters
 * URL: /clothes-product/shirt/pants/hat/coat/socks
 */
app.get('/clothes-product/*', (req, res) => {
    res.send(req.params);
})

/**
 * Use ? to specify optional parameters
 */
app.get('/product-list/:length?', (req, res) => {
  res.send(req.params);
})

/**
 * Promise support
 * In Express 4, if the rejection or error is not explicitly handled
 * or passed to error handling middleware, the app will crash.
 */
app.get('/user-request/reject', () => {
  return Promise.reject('rejected');
});

// app.get('/user-request/reject2', () => {
//   return Promise.reject('rejected').catch((error) => {
//     console.log('Self handle error:', error);
//   })
// });

app.get('/user-request/throw', async () => {
  return await Promise.resolve().then(() => {
      throw new Error('error');
  });
});

// app.get('/user-request/throw2', async () => {
//   return await Promise.resolve().then(() => {
//       throw new Error('error');
//   }).catch((error) => {
//     console.log('Self handle error:', error);
//   });
// });


/**
 * body-parser & req.body changes
 * Keep bodyParser() with deprecated warning
 * Can’t customize the urlencoded body depth (default value = 500)
 * Default “extended” property of urlencoded parser is true
 * req.body initialized to {} by default
 */
app.use(bodyParser());

app.post('/user-info', (req, res) => {
  res.send(`User data: ${JSON.stringify(req.body)}`);
});

app.post('/product-info/', (req, res) => {
  console.log('req.body',req.body);
  if (!req.body) {
    res.status(400).send('No data sent');
  } else {
    res.send(`Product data: ${JSON.stringify(req.body)}`);
  }
});


/**
 * app.router
 * app.router object was removed in Express 4
 */
// const router = app.router;

// router.get('/about', (req, res) => {
//   res.send('Hi!');
// });


/**
 * req.host
 * The Port was removed from the req.host in Express 4
 */
app.get('/dashboard', (req, res) => {
  console.log('Log req.host: ', req.host);
  res.status(200).send(req.host);
})


/**
 * query parser & req.query
 * Default Query Parser is “extended”
 * req.query can be overwritten
 * sample URL: /employee-info?person[name]=bobby&person[age]=25
 */
app.get('/employee-info', (req, res)=> {
  req.query = { error: 'Error message: Not Found!'};
  res.send(req.query);
})


/**
 * req.status
 * For inputs outside this range 
 * OR For non integer inputs → will throw an implicit RangeError
 */
app.get('/request/invalid', (req, res) => {
    res.status(99).send('Invalid status');
    // res.status(1000).send('Invalid status');
    // res.status('a').send('Invalid status');
    //   res.status(500).send('Invalid status');
})


/*
 * ********************************************************************************************************************************
 * Other Updates - Removed methods and properties
 * ********************************************************************************************************************************
 */

/**
 * res.redirect('back') and res.location('back')
 */
app.get('/old-redirect', (req, res) => {
  // Redirecting back to the referrer or the previous page
  res.redirect('back');
});

app.get('/old-location', (req, res) => {
  // Setting the location header to "back"
  res.location('back').send('Redirected to the previous page');
});


/**
 * Remove Methods and Properties
 * app.del -> app.delete
 * req.param(name) -> req.params
 * req.json(status, object) -> res.status(status).json(object)
 * req.send(status, object) -> res.status(status).send(object) 
 * req.send(status) -> req.sendStatus(status)
 */
app.del('/payment-item-delete/:id', (req, res)=> {
    console.log('Log id: ', req.param('id'));
    res.send(500, { error: 'Internal Server Error'});
})


/**
 * app.param(fn) -> app.param('name, fn)
 * customizing the behavior of app.param()
 */
app.param((option) => {
    return (req, res, next, val) => {
        if (val == option) {
            res.json(500, { error: 'Selected option is duplicated with SPECIAL_OPTION' });
        } else {
            next();
        }
    };
});

// using the customized app.param()
const SPECIAL_OPTION = 1337;
app.param('option', SPECIAL_OPTION);

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
app.get('/test-sendfile', function(req, res) {
  req.acceptsCharset();
  req.acceptsEncoding();
  req.acceptsLanguage();
  res.sendfile(path.join(__dirname, '/public/test.html'));
});

app.get('/test-json', function (req, res) {
  // res.json({ user: 'user1' }, 200); // deprecated
  res.json(200,{ user: 'user1' });
});


/**
 * pathIsAbsolute(string) -> path.isAbsolute(string)
 */
app.get('test-path-absolute', (req, res) => {
  // const isAbsolute = pathIsAbsolute('C:/Users');
  const isAbsolute = pathIsAbsolute('src/db');
  // const isAbsolute = path.isAbsolute('src/db')

  console.log('pathIsAbsolute: ',isAbsolute);
  res.sendStatus(200);
})


app.listen(port, () => {
  console.log('Server is up on port ' + port)
})