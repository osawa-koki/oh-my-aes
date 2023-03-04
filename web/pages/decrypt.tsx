import { useState } from "react";
import { Alert, Button, Form } from "react-bootstrap";
import Layout from "../components/Layout";
import setting from "../setting";
import { MyResponseType } from "../src/ResponseType";

export default function EncryptPage() {

  const [content, setContent] = useState<string>('7hDnFrscaucE3pHHLfvLEdufK2/irI78ta06TXKUn3w=');
  const [key, setKey] = useState<string>('my-key');
  const [decrypted, setDecrypted] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);

  const Encrypt = async () => {
    try {
      fetch(`${setting.apiPath}/api/cipher/aes/ecb/decrypt/256?key=${key}&data=${content}`)
        .then(res => res.json())
        .then((json: MyResponseType) => {
          setDecrypted(json.decrypted);
        })
        .catch(e => {
          setError(e.message);
          console.log("###");

        });
    } catch (e) {
      setError(e.message);
    }
  };

  return (
    <Layout>
      <div id="Encrypt" className="mt-3">
        <h1>復号ツール</h1>
        <Form.Group className="mt-3">
          <Form.Label>暗号化された文字列</Form.Label>
          <Form.Control as="textarea" rows={5} value={content} onInput={(e) => {setContent((e.target as HTMLTextAreaElement).value)}} />
        </Form.Group>
        <Form.Group className="mt-3">
          <Form.Label>暗号化キー</Form.Label>
          <Form.Control type="text" placeholder="Enter Key" value={key} onInput={(e) => {setKey((e.target as HTMLTextAreaElement).value)}} />
        </Form.Group>
        <div className="mt-3 d-flex justify-content-center">
          <Button variant="primary" onClick={Encrypt}>復号 🔏</Button>
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
              decrypted !== null && (
                <>
                  <Alert variant="info" className="mt-3">
                  復号された文字列は以下の通りです。
                  </Alert>
                  <Form.Group className="mt-3">
                    <Form.Label>復号された文字列</Form.Label>
                    <Form.Control readOnly as="textarea" value={decrypted} rows={5} />
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
