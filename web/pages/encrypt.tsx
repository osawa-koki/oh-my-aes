import { useState } from "react";
import { Alert, Button, Form } from "react-bootstrap";
import Layout from "../components/Layout";
import setting from "../setting";
import { MyResponseType } from "../src/ResponseType";

export default function EncryptPage() {

  const [content, setContent] = useState<string>('こんにちは！');
  const [key, setKey] = useState<string>('my-key');
  const [encrypted, setEncrypted] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);

  const Encrypt = async () => {
    try {
      fetch(`${setting.apiPath}/api/cipher/aes/ecb/encrypt/256?key=${key}&data=${content}`)
        .then(res => res.json())
        .then((json: MyResponseType) => {
          setEncrypted(json.encrypted);
        })
        .catch(e => {
          setError(e.message);
        });
    } catch (e) {
      setError(e.message);
    }
  };

  return (
    <Layout>
      <div id="Encrypt" className="mt-3">
        <h1>暗号化ツール</h1>
        <Form.Group className="mt-3">
          <Form.Label>暗号化対象文字列</Form.Label>
          <Form.Control as="textarea" rows={5} value={content} onInput={(e) => {setContent((e.target as HTMLTextAreaElement).value)}} />
        </Form.Group>
        <Form.Group className="mt-3">
          <Form.Label>暗号化キー</Form.Label>
          <Form.Control type="text" placeholder="Enter Key" value={key} onInput={(e) => {setKey((e.target as HTMLTextAreaElement).value)}} />
        </Form.Group>
        <div className="mt-3 d-flex justify-content-center">
          <Button variant="primary" onClick={Encrypt}>暗号化 🔏</Button>
        </div>
        {
          error !== null ? (
            <>
              <Alert variant="danger" className="mt-3">
                {error}
              </Alert>
            </>
          ) : (
            <>
            {
              encrypted !== null && (
                <>
                  <Alert variant="info" className="mt-3">
                    暗号化された文字列は以下の通りです。
                  </Alert>
                  <Form.Group className="mt-3">
                    <Form.Label>暗号化された文字列</Form.Label>
                    <Form.Control readOnly as="textarea" value={encrypted} rows={5} />
                  </Form.Group>
                </>
              )
            }
            </>
          )
        }
      </div>
    </Layout>
  );
};
