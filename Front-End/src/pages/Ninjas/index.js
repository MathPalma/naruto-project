import React, { useState, useEffect } from 'react';
import { Link, useHistory } from 'react-router-dom';
import { FiEdit, FiTrash2 } from 'react-icons/fi';

import api from '../../services/api'

import './styles.css';
import { Col, Container, Row } from 'react-bootstrap';

export default function Ninjas() {

    const [ninjas, setNinjas] = useState([]);

    const history = useHistory();

    useEffect(() => {
        api.get('/Naruto/asc/20/1')
            .then(response => {
                setNinjas(response.data.list)
            })
    });

    async function EditNinja(id) {
        try {
            history.push(`ninja/new/${id}`)
        } catch (err) {
            alert('Edit ninja failed! Try again');
        }
    }

    async function DeleteNinja(id, imageName) {
        try {
            await api.delete(`Naruto/${id}/${imageName}`);

            setNinjas(ninjas.filter(ninja => ninja.id != id))
        } catch (err) {
            console.log(err);
            alert('Delete failed! Try again')
        }
    }

    return (
        <div>
            <div className="header">
                <Container className="mt-auto">
                    <Row>
                        <Col><label>Registered Ninjas</label></Col>
                        <Col className="d-flex justify-content-end"><Link className="button" to="ninja/new/0">Add new Ninja</Link></Col>
                    </Row>
                </Container>
            </div>
            <div className="content">
                <Container>
                    <Row>
                        <ul>
                            {ninjas.map(ninja => (
                                <li key={ninja.id}>
                                    <div className="ninja-infos">
                                        <strong>Name:</strong>
                                        <p>{ninja.name}</p>
                                        <strong>Village:</strong>
                                        <p>{ninja.village}</p>
                                        <strong>Rank:</strong>
                                        <p>{ninja.rank}</p>
                                    </div>
                                    <div className="ninja-image">
                                        <img src={ninja.imageSrc} alt="" />
                                    </div>

                                    <button onClick={() => EditNinja(ninja.id)}
                                        type="button">
                                        <FiEdit size={20} color="#FF6347" />
                                    </button>

                                    <button id="delete-button" onClick={() => DeleteNinja(ninja.id, ninja.imageName)}
                                        type="button">
                                        <FiTrash2 size={20} color="#FF6347" />
                                    </button>

                                </li>
                            ))}
                        </ul>
                    </Row>
                </Container>
            </div>

        </div>
    )
}