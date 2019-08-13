import fetch from "node-fetch";
import {NextApiRequest, NextApiResponse} from 'next';

export default (req: NextApiRequest, res: NextApiResponse) => {
    if(req.method === 'POST') {
        const data = JSON.parse(req.body);
        const dto = {
            data
        };
        const json = JSON.stringify(dto);
        console.log(json);
        
        fetch('https://ckzvgrbymezqegu.form.io/reacttestform/submission', {
            method:'post',
            headers: {'x-auth': 'react-test'},
            body: json
        }).then(() => {
            res.setHeader('Content-Type', 'application/json');
            res.statusCode = 200;
            res.end(JSON.stringify({status: 'ok'}));
        }).catch(err => {
            res.statusCode = 500;
            res.end(err.toString());
        });
    }
};
  