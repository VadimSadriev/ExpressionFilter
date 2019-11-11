import React, { Component } from 'react';
import { Table, FormGroup, Label, Input, Form, Button, Card, CardBody, CardHeader } from 'reactstrap';

export default class Home extends Component {

    constructor(props) {
        super(props);

        this.state = {
            showCreatePanel: false,
            characterTypes: [],
            character: {
                name: "",
                type: ""
            }
        }
    }

    componentWillMount() {

        fetch('api/home/getsecondarydata')
            .then(res => res.json())
            .then(res => {
                this.setState({
                    characterTypes: res.map(x => x.value)
                })
            })
            .catch(res => {
                console.log(res);
            })
    }

    showPanel = () => this.setState({
        showCreatePanel: true,
        character: {
            type: this.state.characterTypes[0]
        }
    });

    onNameChanged = (e) => {
        this.setState({
            character: {
                name: e.target.value,
                type: this.state.character.type,
            }
        })
    }

    onTypeChanged = (e) => {
        this.setState({
            character: {
                name: this.state.character.name,
                type: e.target.value
            }
        })
    }

    render() {
        return (
            <div>
                <h1>Hello, world!</h1>

                <div className="row">
                    <div className="col-sm-12 col-lg-6 col-md-6">
                        <div className="mb-1">
                            <Button outline color="primary" size="sm" onClick={this.showPanel}>Create character</Button>
                        </div>
                        <Table responsive>
                            <thead>
                                <tr>
                                    <th>Name</th>
                                </tr>
                            </thead>
                            <tbody>

                            </tbody>
                        </Table>
                    </div>
                    {
                        this.state.showCreatePanel
                            ? <div className="col-sm-12 col-md-6 col-lg-6">
                                <Card>
                                    <CardHeader>Create panel <Button close onClick={() => this.setState({ showCreatePanel: false })} /></CardHeader>
                                    <CardBody>
                                        <Form>
                                            <FormGroup>
                                                <Label for="character-name">Name</Label>
                                                <Input id="character-name" placeholder="type character name" onChange={this.onNameChanged} />
                                            </FormGroup>
                                            <FormGroup>
                                                <Label for="character-type">Character type</Label>
                                                <Input type="select" id="character-type" onChange={this.onTypeChanged}>
                                                    {
                                                        this.state.characterTypes.map(x => {
                                                            return <option key={x} >{x}</option>;
                                                        })
                                                    }
                                                </Input>
                                            </FormGroup>
                                            <Button outline color="success">Create</Button>
                                        </Form>
                                    </CardBody>
                                </Card>
                            </div>
                            : ""
                    }
                </div>
            </div>
        );
    }
}
