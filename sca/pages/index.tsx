import '../styles/index.less';
import { useState, useReducer } from 'react';

const FIRST_NAME = 'firstName';
const LAST_NAME = 'lastName';
const PHONE = 'mobilePhone';
const EMAIL = 'email';
const API_ENDPOINT = '/api/subscribe';
const FIELDS = [FIRST_NAME, LAST_NAME, PHONE, EMAIL];

interface FormState {
    data: Map<string, ControlValidityWithName>;
    isValid: boolean;
}

const initialState: FormState = {
    isValid: false,
    data: new Map<string, ControlValidityWithName>()
};
FIELDS.forEach(fieldName => initialState.data.set(fieldName, {isValid: false, text: '', name: fieldName}));

interface ControlValidity {
    text: string;
    isValid: boolean;
}

interface ControlValidityWithName extends ControlValidity {
    name: string;
}

interface TextInputProps {
    label: string;
    type: string;
    isRequired?: boolean;
    onValidChange: (valid: ControlValidity) => void;
}

function TextInput(props: TextInputProps) {
    const [text, setText] = useState('');
    const [isValid, setIsValid] = useState(!props.isRequired);
    const className = isValid ? 'valid' : 'invalid';

    return <label>
        <span>{props.label}</span>
        {props.isRequired ? <span className="required">*</span> : null}
        <input type={props.type} className={className} value={text} required={props.isRequired} onChange={e => {
            const text = e.currentTarget.value.trim();
            setText(text);
            const isValid = !(props.isRequired && !text.length);
            setIsValid(isValid);
            props.onValidChange({isValid, text});
         }} />
    </label>;
}

function stateReducer(state: FormState, action: ControlValidityWithName) {
    const ret = {...state};
    ret.data.set(action.name, action);
    ret.isValid = Array.from(ret.data.values()).every(v => v.isValid);
    return ret;
}

function Home() {
    const [hasPosted, setHasPosted] = useState(false);
    const [error, setError] = useState(null);
    const [formState, setFormState] = useReducer(stateReducer, initialState);
    const btnClassName = formState.isValid ? 'valid' : 'invalid';

    return <article>
        <header>Subscribe</header>
        {error 
            ? <div className="hero error">{error}</div>
            : hasPosted 
                ? <div className="hero">Thank you for subscribing!</div>
                : <form method="post" action={API_ENDPOINT} onSubmit={e => {
                    e.preventDefault();
                    const dto = {};
                    for(let item of Array.from(formState.data.values())) {
                        dto[item.name] = item.text;
                    }
                    console.log(dto);
                    fetch(API_ENDPOINT, {
                        body: JSON.stringify(dto),
                        method: "post"
                    })
                    .then(async resp => resp.ok ? setHasPosted(true) : setError(await resp.text()))
                    .catch(err => setError(err.toString()));
                }}>
                    <div className="controls">
                        <TextInput label="First Name" type="text" isRequired={true} onValidChange={c => setFormState({name: FIRST_NAME, ...c})}/>
                        <TextInput label="Last Name" type="text" isRequired={true} onValidChange={c => setFormState({name: LAST_NAME, ...c})}/>
                        <TextInput label="Email" type="email" isRequired={true} onValidChange={c => setFormState({name: EMAIL, ...c})}/>
                        <TextInput label="Mobile Phone" type="text" onValidChange={c => setFormState({name: PHONE, ...c})} />
                    </div>
                    <input type="submit" value="Submit" className={btnClassName} disabled={!formState.isValid} />
                </form>
        }
    </article>;
}
  
export default Home;
  