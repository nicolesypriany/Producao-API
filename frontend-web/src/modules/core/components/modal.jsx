import { Button, Modal } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";

const RegisterModal = ({ children, title }) => {
  const [opened, { open, close }] = useDisclosure(false);
  return (
    <>
      <Modal opened={opened} onClose={close} title={title} centered>
        {children}
      </Modal>

      <Button variant="filled" w="18%" size="sm" onClick={open}>
        {title}
      </Button>
    </>
  );
};

export default RegisterModal;
